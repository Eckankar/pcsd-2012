using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeyValueBase.Interfaces;
using KeyValueBase.Faults;
using System.IO;
using System.Globalization;
using System.Reflection;

namespace KeyValueBase
{
    public class KeyValueBaseReplicaImpl : IKeyValueBaseReplica<KeyImpl, ValueListImpl>
    {
        protected static object syncInitObj = new Object();
        protected static bool initializing;

        protected static StoreImpl store;
        protected static IndexImpl index;
        protected static TimestampLog stamp;

        public void Init(string serverFilename)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            if (codeBase.StartsWith("file:"))
                codeBase = codeBase.Substring(5).TrimStart('/', '\\');
            if (serverFilename.Contains(":"))
            {
                serverFilename = Path.GetFullPath(serverFilename);
            }
            else
            {
                serverFilename = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(codeBase), "..", "data", serverFilename));
            }
            if (!File.Exists(serverFilename))
                throw new FileNotFoundException("Store initialization file not found", serverFilename);
            lock (syncInitObj)
            {
                if (initializing)
                    throw new ServiceInitializingException();
                initializing = true;
            }
            if (index != null)
                throw new ServiceAlreadyInitializedException();
            store = new StoreImpl(1024L * 1024L * 10L);
            index = new IndexImpl(store);
            LoadIndexFromFile(serverFilename);

            lock (syncInitObj)
            {
                initializing = false;
            }
        }

        private static readonly char[] whiteSpace = new char[] { ' ', '\t' };

        // Assumes that all values of same key are on consecutive lines.
        private void LoadIndexFromFile(string serverFilename)
        {
            int? lastKey = null;
            List<int> values = new List<int>();
            LogRecord lr = new LogRecord(OperationType.Insert, null);
            stamp = lr.Lsn;

            using (StreamReader sr = new StreamReader(serverFilename))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.First(c => !whiteSpace.Contains(c)) == '#')
                        continue;
                    string[] parts = line.Split(whiteSpace, StringSplitOptions.RemoveEmptyEntries);
                    int key = Int32.Parse(parts[0], CultureInfo.InvariantCulture);
                    int value = Int32.Parse(parts[1], CultureInfo.InvariantCulture);

                    if (!lastKey.HasValue) lastKey = key;
                    else if (key != lastKey)
                    {
                        index.Insert(new KeyImpl(lastKey.Value), new ValueListImpl(values.Select(v => new ValueImpl(v))));
                        values.Clear();
                        lastKey = key;
                    }

                    values.Add(value);
                }
            }
        }

        private void CheckInitialized()
        {
            if (index == null)
                throw new ServiceNotInitializedException();
        }
        public KeyValuePair<TimestampLog, ValueListImpl> Read(KeyImpl k)
        {
            CheckInitialized();
            lock (stamp)
            {
                return new KeyValuePair<TimestampLog, ValueListImpl>(stamp, index.Get(k));
            }
        }

        public KeyValuePair<TimestampLog, IEnumerable<ValueListImpl>> Scan(KeyImpl begin, KeyImpl end, IPredicate<ValueListImpl> p)
        {
            CheckInitialized();
            var b = index.Scan(begin, end).Where(v => p.Evaluate(v));
            lock(stamp) {
                return new KeyValuePair<TimestampLog, IEnumerable<ValueListImpl>>(stamp, b);
            }
        }

        public KeyValuePair<TimestampLog, IEnumerable<ValueListImpl>> AtomicScan(KeyImpl begin, KeyImpl end, IPredicate<ValueListImpl> p)
        {
            CheckInitialized();
            var a = index.AtomicScan(begin, end).Where(v => p.Evaluate(v));
            lock (stamp)
            {
                return new KeyValuePair<TimestampLog, IEnumerable<ValueListImpl>>(stamp, a);
            }
        }
    }
}

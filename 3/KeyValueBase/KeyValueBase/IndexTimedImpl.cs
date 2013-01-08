using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeyValueBase.Interfaces;
using KeyValueBase.Faults;
using System.Collections.Concurrent;

namespace KeyValueBase {
    public class IndexTimedImpl : IIndexTimed<KeyImpl, ValueListImpl> {
        private ConcurrentDictionary<KeyImpl, Object> locks;
        private Dictionary<KeyImpl, AllocRecord> dict;
        private Dictionary<KeyImpl, TimestampLog> timestamps;

        private IStore appleStore;
        private int nextFree;
        private ValueListSerializerImpl serializer;

        private class AllocRecord {
            public int position, size;

            public AllocRecord(int position, int size) {
                this.position = position;
                this.size = size;
            }
        }


        public IndexTimedImpl(IStore appleStore) {
            dict = new Dictionary<KeyImpl, AllocRecord>();
            timestamps = new Dictionary<KeyImpl, TimestampLog>();
            locks = new ConcurrentDictionary<KeyImpl, object>();
            this.appleStore = appleStore;
            nextFree = 0;
            serializer = new ValueListSerializerImpl();
        }

        public void Insert(KeyImpl key, TimestampLog lsn, ValueListImpl values) {
            Object o = new Object();
            if(!locks.TryAdd(key, o)) {
                throw new KeyAlreadyPresentException<KeyImpl>(key);
            }

            lock (o) {
                byte[] data = serializer.ToByteArray(values);
                appleStore.Write(nextFree, data);
                dict[key] = new AllocRecord(nextFree, data.Length);
                timestamps[key] = lsn;
                nextFree += data.Length;
                return;
            }
        }

        public void Remove(KeyImpl key) {
            Object o, a;
            if(!locks.TryGetValue(key, out o)) {
                throw new KeyNotFoundException<KeyImpl>(key);
            }
            lock (o) {
                locks.TryRemove(key, out a);
                timestamps.Remove(key);
                dict.Remove(key);
            }
        }

        public void Update(KeyImpl key, TimestampLog lsn, ValueListImpl newValues)
        {
            Object o;
            if (!locks.TryGetValue(key, out o))
            {
                throw new KeyNotFoundException<KeyImpl>(key);
            }
            lock (o) {
                this.Remove(key);
                this.Insert(key, lsn, newValues);
            }
        }

        public KeyValuePair<TimestampLog, ValueListImpl> Get(KeyImpl key) {
            Object o;
            if (!locks.TryGetValue(key, out o)) {
                throw new KeyNotFoundException<KeyImpl>(key);
            }
            lock (o) {
                AllocRecord record = dict[key];
                byte[] data = appleStore.Read(record.position, record.size);
                ValueListImpl values = serializer.FromByteArray(data);
                return new KeyValuePair<TimestampLog,ValueListImpl>(timestamps[key], values);
            }
        }

        public KeyValuePair<TimestampLog, IEnumerable<ValueListImpl>> Scan(KeyImpl begin, KeyImpl end) {
            if (begin.Key > end.Key) {
                throw new BeginGreaterThanEndException<KeyImpl>(begin, end);
            }

            var values = from k in dict.Keys
                         where k.Key >= begin.Key && k.Key <= end.Key
                         select Get(k);

            var ts = from t in timestamps
                         where t.Key.Key >= begin.Key && t.Key.Key <= end.Key
                         select t;
            

            return new KeyValuePair<TimestampLog,IEnumerable<ValueListImpl>>(ts.First().Value, (IEnumerable<ValueListImpl>)values.ToList());
        }

        public KeyValuePair<TimestampLog, IEnumerable<ValueListImpl>> AtomicScan(KeyImpl begin, KeyImpl end)
        {        
            var ls = from k in locks.Keys
                        where k.Key >= begin.Key && k.Key <= end.Key
                        select Get(k);

            try {
                foreach (Object o in ls) {
                    System.Threading.Monitor.Enter(o);
                }
                return Scan(begin, end);
            }
            finally {
                foreach (Object o in ls) {
                    System.Threading.Monitor.Exit(o);
                }
            }
        }

        public void BulkPut(IEnumerable<KeyValuePair<KeyImpl, ValueListImpl>> pairs, TimestampLog lsn) {
            lock (dict) {
                foreach (KeyValuePair<KeyImpl, ValueListImpl> pair in pairs) {
                    if (dict.ContainsKey(pair.Key)) {
                        Update(pair.Key, lsn, pair.Value);
                    } else {
                        Insert(pair.Key, lsn, pair.Value);
                    }
                }
            }
        }
    }
}

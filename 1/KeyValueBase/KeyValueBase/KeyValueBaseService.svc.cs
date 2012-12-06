﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeyValueBase.Interfaces;
using KeyValueBase.Faults;
using System.IO;
using System.Globalization;

namespace KeyValueBase {
    public class KeyValueBaseService : IKeyValueBase<KeyImpl, ValueListImpl> {
        private static object syncInitObj = new Object();
        private static bool initializing;

        private static StoreImpl store;
        private static IndexImpl index;

        public void Init(string serverFilename) {
            if (!File.Exists(serverFilename))
                throw new FileNotFoundException("Store initialization file not found", serverFilename);
            lock (syncInitObj) {
                if (initializing)
                    throw new ServiceInitializingException();
                initializing = true;
            }
            if (index != null)
                throw new ServiceAlreadyInitializedException();
            store = new StoreImpl(1024*1024*10);
            index = new IndexImpl(store);
            LoadIndexFromFile(serverFilename);

            lock (syncInitObj) {
                initializing = false;
            }
        }

        private static readonly char[] whiteSpace = new char[] { ' ', '\t' };

        private void LoadIndexFromFile(string serverFilename) {
            Dictionary<int, List<int>> keyValues = new Dictionary<int,List<int>>();
            using (StreamReader sr = new StreamReader(serverFilename)) {
                string line;
                while ((line = sr.ReadLine()) != null) {
                    string[] parts = line.Split(whiteSpace, StringSplitOptions.RemoveEmptyEntries);
                    int key = Int32.Parse(parts[0], CultureInfo.InvariantCulture);
                    int value = Int32.Parse(parts[1], CultureInfo.InvariantCulture);
                    List<int> valueList;
                    if (!keyValues.TryGetValue(key, out valueList)) {
                        valueList = new List<int>(new int[] { value });
                        keyValues.Add(key, valueList);
                    }
                    else {
                        valueList.Add(value);
                    }
                }
            }
            foreach (var kv in keyValues) {
                index.Insert(new KeyImpl(kv.Key), new ValueListImpl(kv.Value.Select(v => new ValueImpl(v))));
            }
        }

        private void CheckInitialized() {
            if (index == null)
                throw new ServiceNotInitializedException();
        }

        public ValueListImpl Read(KeyImpl key) {
            CheckInitialized();
            return index.Get(key);
        }

        public void Insert(KeyImpl key, ValueListImpl value) {
            CheckInitialized();
            index.Insert(key, value);
        }

        public void Update(KeyImpl key, ValueListImpl newValue) {
            CheckInitialized();
            index.Update(key, newValue);
        }

        public void Delete(KeyImpl key) {
            CheckInitialized();
            index.Remove(key);
        }

        public IEnumerable<ValueListImpl> Scan(KeyImpl begin, KeyImpl end, IPredicate<ValueListImpl> predicate) {
            CheckInitialized();
            return index.Scan(begin, end).Where(v => predicate.Evaluate(v));
        }

        public IEnumerable<ValueListImpl> AtomicScan(KeyImpl begin, KeyImpl end, IPredicate<ValueListImpl> predicate) {
            CheckInitialized();
            throw new NotImplementedException();
        }

        public void BulkPut(IEnumerable<KeyValuePair<KeyImpl, ValueListImpl>> pairs) {
            CheckInitialized();
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeyValueBase.Interfaces;
using KeyValueBase.Faults;

namespace KeyValueBase {
    public class IndexImpl : IIndex<KeyImpl, ValueListImpl> {
        private Dictionary<KeyImpl, ValueListImpl> dict;

        public void Insert(KeyImpl key, ValueListImpl value) {
            lock (dict) {
                dict.Add(key, value);
            }
        }

        public void Remove(KeyImpl key) {
            lock (dict) {
                dict.Remove(key);
            }
        }

        public void Update(KeyImpl key, ValueListImpl newValue) {
            lock (dict) {
                if (!dict.ContainsKey(key)) throw new ArgumentException("key");

                dict[key] = newValue;
            }
        }

        public ValueListImpl Get(KeyImpl key) {
            lock (dict) {
                return dict[key];
            }
        }

        public IEnumerable<ValueListImpl> Scan(KeyImpl begin, KeyImpl end) {
            lock (dict) {
                var values = from k in dict.Keys
                             where k.Key >= begin.Key && k.Key <= end.Key
                             select dict[k];

                return values;
            }
        }

        public IEnumerable<ValueListImpl> AtomicScan(KeyImpl begin, KeyImpl end) {
            throw new NotImplementedException();
        }

        public void BulkPut(IEnumerable<KeyValuePair<KeyImpl, ValueListImpl>> pairs) {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeyValueBase.Interfaces;
using KeyValueBase.Faults;

namespace KeyValueBase {
    public class IndexImpl : IIndex<KeyImpl, ValueListImpl> {
        private Dictionary<KeyImpl, ValueListImpl> dict;

        public IndexImpl() {
            dict = new Dictionary<KeyImpl, ValueListImpl>();
        }

        public void Insert(KeyImpl key, ValueListImpl value) {
            try {
                lock (dict) {
                    dict.Add(key, value);
                }
            }
            catch (ArgumentException) {
                throw new KeyAlreadyPresentException<KeyImpl>(key);
            }
        }

        public void Remove(KeyImpl key) {
            lock (dict) {
                if (!dict.Remove(key))
                    throw new KeyNotFoundException<KeyImpl>(key);
            }
        }

        public void Update(KeyImpl key, ValueListImpl newValue) {
            lock (dict) {
                if (!dict.ContainsKey(key))
                    throw new KeyNotFoundException<KeyImpl>(key);

                dict[key] = newValue;
            }
        }

        public ValueListImpl Get(KeyImpl key) {
            try {
                lock (dict) {
                    return dict[key];
                }
            }
            catch (KeyNotFoundException) {
                throw new KeyNotFoundException<KeyImpl>(key);
            }
        }

        public IEnumerable<ValueListImpl> Scan(KeyImpl begin, KeyImpl end) {
            if (begin.Key > end.Key)
                throw new BeginGreaterThanEndException<KeyImpl>(begin, end);
            
            //Do we need this??
            /*if (!dict.ContainsKey(begin))
                throw new KeyNotFoundException<KeyImpl>(begin);
            if (!dict.ContainsKey(end))
                throw new KeyNotFoundException<KeyImpl>(end);*/
            var values = from k in dict.Keys
                            where k.Key >= begin.Key && k.Key <= end.Key
                            select dict[k];

            return values;
        }

        public IEnumerable<ValueListImpl> AtomicScan(KeyImpl begin, KeyImpl end) {
            lock (dict) {
                return Scan(begin, end);
            }
        }

        public void BulkPut(IEnumerable<KeyValuePair<KeyImpl, ValueListImpl>> pairs) {
            throw new NotImplementedException();
        }
    }
}

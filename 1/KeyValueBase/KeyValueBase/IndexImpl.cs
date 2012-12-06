using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeyValueBase.Interfaces;
using KeyValueBase.Faults;

namespace KeyValueBase {
    public class IndexImpl : IIndex<KeyImpl, ValueListImpl> {
        private Dictionary<KeyImpl, AllocRecord> dict;
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


        public IndexImpl() {
            dict = new Dictionary<KeyImpl, AllocRecord>();
            appleStore = new StoreImpl(100000000);
            nextFree = 0;
            serializer = new ValueListSerializerImpl();
        }

        public void Insert(KeyImpl key, ValueListImpl values) {
            lock (dict) {
                if (dict.ContainsKey(key)) {
                    throw new KeyAlreadyPresentException<KeyImpl>(key);
                }

                byte[] data = serializer.ToByteArray(values);
                appleStore.Write(nextFree, data);
                dict[key] = new AllocRecord(nextFree, data.Length);
                nextFree += data.Length;
                return;
            }
        }

        public void Remove(KeyImpl key) {
            lock (dict) {
                if (!dict.Remove(key)) {
                    throw new KeyNotFoundException<KeyImpl>(key);
                }
            }
        }

        public void Update(KeyImpl key, ValueListImpl newValues) {
            lock (dict) {
                this.Remove(key);
                this.Insert(key, newValues);
            }
        }

        public ValueListImpl Get(KeyImpl key) {
            lock (dict) {
                if (!dict.ContainsKey(key)) {
                    throw new KeyNotFoundException<KeyImpl>(key);
                }

                AllocRecord record = dict[key];
                byte[] data = appleStore.Read(record.position, record.size);
                ValueListImpl values = serializer.FromByteArray(data);
                return values;
            }
        }

        public IEnumerable<ValueListImpl> Scan(KeyImpl begin, KeyImpl end) {
            if (begin.Key > end.Key) {
                throw new BeginGreaterThanEndException<KeyImpl>(begin, end);
            }

            //Do we need this??
            /*if (!dict.ContainsKey(begin))
                throw new KeyNotFoundException<KeyImpl>(begin);
            if (!dict.ContainsKey(end))
                throw new KeyNotFoundException<KeyImpl>(end);*/
            var values = from k in dict.Keys
                         where k.Key >= begin.Key && k.Key <= end.Key
                         select Get(k);

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeyValueBase.Interfaces;

namespace KeyValueBase {
    public class ValueSerializerImpl : IValueSerializer<ValueImpl> {
        private IStore appleStore;
        private List<AllocRecord> allocated;
        private int nextFree;

        private class AllocRecord {
            public int position, size;

            public AllocRecord(int position, int size) {
                this.position = position;
                this.size = size;
            }
        }

        public ValueSerializerImpl(IStore appleStore) {
            this.appleStore = appleStore;
            allocated = new List<AllocRecord>();
            nextFree = 0;
        }

        public ValueImpl FromByteArray(byte[] array) {
            lock (appleStore) {
                ValueImpl value = new ValueImpl(array.Length - 1);
                appleStore.Write(nextFree, array);
                allocated.Add(new AllocRecord(nextFree, array.Length));
                nextFree += array.Length;

                return value;
            }
        }

        public byte[] ToByteArray(ValueImpl v) {
            lock (appleStore) {
                AllocRecord rec = allocated[v.Value];

                return appleStore.Read(rec.position, rec.size);
            }
        }
    }
}

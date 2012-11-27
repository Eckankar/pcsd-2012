using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeyValueBase.Interfaces;

namespace KeyValueBase {
    public class KeyValueBaseImpl : IKeyValueBase<KeyImpl, ValueListImpl> {
        public void Init(string serverFilename) {
            throw new NotImplementedException();
        }

        public ValueListImpl Read(KeyImpl key) {
            throw new NotImplementedException();
        }

        public void Insert(KeyImpl key, ValueListImpl value) {
            throw new NotImplementedException();
        }

        public void Update(KeyImpl key, ValueListImpl newValue) {
            throw new NotImplementedException();
        }

        public void Delete(KeyImpl key) {
            throw new NotImplementedException();
        }

        public IEnumerable<ValueListImpl> Scan(KeyImpl begin, KeyImpl end, IPredicate<ValueListImpl> predicate) {
            throw new NotImplementedException();
        }

        public IEnumerable<ValueListImpl> AtomicScan(KeyImpl begin, KeyImpl end, IPredicate<ValueListImpl> predicate) {
            throw new NotImplementedException();
        }

        public void BulkPut(IEnumerable<KeyValuePair<KeyImpl, ValueListImpl>> pairs) {
            throw new NotImplementedException();
        }
    }
}

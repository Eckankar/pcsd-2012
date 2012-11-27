using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeyValueBase.Interfaces;

namespace KeyValueBase {
    public class ValueListImpl : IValueList<ValueImpl> {
        private List<ValueImpl> list;

        public ValueListImpl() {
            list = new List<ValueImpl>();
        }

        public ValueListImpl(IEnumerable<ValueImpl> values) {
            list = new List<ValueImpl>(values);
        }

        public void Add(ValueImpl item) {
            lock (list) {
                list.Add(item);
            }
        }

        public void Remove(ValueImpl item) {
            lock (list) {
                list.Remove(item);
            }
        }

        public void Merge(IValueList<ValueImpl> other) {
            lock (list) {
                list.AddRange(other);
            }
        }

        public IList<ValueImpl> ToList() {
            return list;
        }

        // Not thread safe yet.
        public bool Equals(IValue other) {
            if (!(other is IValueList<ValueImpl>)) return false;

            return this.Intersect((IValueList<ValueImpl>)other).Count() == this.Count();
        }

        public IEnumerator<ValueImpl> GetEnumerator() {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return list.GetEnumerator();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeyValueBase.Interfaces;
using System.Runtime.Serialization;

namespace KeyValueBase {
    [DataContract]
    public class ValueListImpl : IValueList<ValueImpl> {
        [DataMember]
        public List<ValueImpl> List { get; private set; }

        public ValueListImpl() {
            List = new List<ValueImpl>();
        }

        public ValueListImpl(IEnumerable<ValueImpl> values) {
            List = new List<ValueImpl>(values);
        }

        public void Add(ValueImpl item) {
            lock (List) {
                List.Add(item);
            }
        }

        public void Remove(ValueImpl item) {
            lock (List) {
                List.Remove(item);
            }
        }

        public void Merge(IValueList<ValueImpl> other) {
            lock (List) {
                List.AddRange(other);
            }
        }

        public IList<ValueImpl> ToList() {
            return List;
        }

        // Not thread safe yet.
        public bool Equals(IValue other) {
            if (!(other is IValueList<ValueImpl>)) return false;

            return this.Intersect((IValueList<ValueImpl>)other).Count() == this.Count();
        }

        public IEnumerator<ValueImpl> GetEnumerator() {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return List.GetEnumerator();
        }
    }
}

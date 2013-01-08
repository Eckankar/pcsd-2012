using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeyValueBase.Interfaces;
using System.Runtime.Serialization;

namespace KeyValueBase {
    [DataContract]
    public class ValueImpl : IValue {
        [DataMember]
        public int Value { get; private set; }

        public ValueImpl(int value) {
            this.Value = value;
        }

        public override bool Equals(object obj) {
            return (obj is ValueImpl) && (this.Value == ((ValueImpl)obj).Value);
        }

        public bool Equals(IValue other) {
            return Equals((object)other);
        }

        public override int GetHashCode() {
            return this.Value.GetHashCode();
        }
    }
}

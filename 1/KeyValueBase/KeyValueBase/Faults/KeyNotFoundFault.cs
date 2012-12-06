using System;
using System.Runtime.Serialization;
using KeyValueBase.Interfaces;
using System.ServiceModel;

namespace KeyValueBase.Faults {
    [DataContract]
    public class KeyNotFoundFault<K>
      where K : IKey<K> {
        [DataMember(Name = "Key")]
        private K key;

        [DataMember(Name = "Message")]
        private string message;

        public KeyNotFoundFault(K key)
            : this(key, String.Format("The key '{0}' is not present", key)) {
        }

        public KeyNotFoundFault(K key, string message) {
            this.key = key;
            this.message = message;
        }

        public K Key {
            get { return this.key; }
        }

        public string Message {
            get { return this.message; }
        }
    }

    public class KeyNotFoundException<K> : FaultException<KeyNotFoundFault<K>> where K : IKey<K> {
        public KeyNotFoundException(K key)
            : base(new KeyNotFoundFault<K>(key)) { }
        public KeyNotFoundException(K key, string message)
            : base(new KeyNotFoundFault<K>(key, message)) { }
    }
}

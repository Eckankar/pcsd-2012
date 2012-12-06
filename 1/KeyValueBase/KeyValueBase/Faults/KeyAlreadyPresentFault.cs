using System;
using System.Runtime.Serialization;
using KeyValueBase.Interfaces;
using System.ServiceModel;

namespace KeyValueBase.Faults {
    [DataContract]
    public class KeyAlreadyPresentFault<K>
      where K : IKey<K> {
        [DataMember(Name = "Key")]
        private K key;

        [DataMember(Name = "Message")]
        private string message;

        public KeyAlreadyPresentFault(K key)
            : this(key, String.Format("The key '{0}' is already present", key)) {
        }

        public KeyAlreadyPresentFault(K key, string message) {
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

    public class KeyAlreadyPresentException<K> : FaultException<KeyAlreadyPresentFault<K>> where K : IKey<K> {
        public KeyAlreadyPresentException(K key)
            : base(new KeyAlreadyPresentFault<K>(key)) { }
        public KeyAlreadyPresentException(K key, string message)
            : base(new KeyAlreadyPresentFault<K>(key, message)) { }
    }
}

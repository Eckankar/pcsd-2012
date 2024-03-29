﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeyValueBase.Interfaces;
using System.Runtime.Serialization;

namespace KeyValueBase {
    [DataContract]
    public class KeyImpl : IKey<KeyImpl> {
        [DataMember]
        public int Key { get; private set; }

        public KeyImpl(int key) {
            this.Key = key;
        }

        public int CompareTo(KeyImpl other) {
            return this.Key.CompareTo(other.Key);
        }

        public override bool Equals(object obj) {
            return (obj is KeyImpl) && (this.Key == ((KeyImpl)obj).Key);
        }

        public override int GetHashCode() {
            return this.Key.GetHashCode();
        }
    }
}

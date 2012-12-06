using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using KeyValueBase.Interfaces;

namespace KeyValueBase {
    [DataContract]
    public class MinLengthPredicate : IPredicate<ValueListImpl> {
        [DataMember]
        public int Length { get; set; }

        public bool Evaluate(ValueListImpl input) {
            return Length >= input.Count();
        }
    }

    [DataContract]
    public class MaxLengthPredicate : IPredicate<ValueListImpl> {
        [DataMember]
        public int Length { get; set; }

        public bool Evaluate(ValueListImpl input) {
            return Length <= input.Count();
        }
    }

    [DataContract]
    public class ContainsPredicate : IPredicate<ValueListImpl> {
        [DataMember]
        public ValueImpl Element { get; set; }

        public bool Evaluate(ValueListImpl input) {
            return input.Contains(Element);
        }
    }

    [DataContract]
    public class AndPredicate<V> : IPredicate<V> {
        [DataMember]
        public IPredicate<V> Predicate1 { get; set; }
        [DataMember]
        public IPredicate<V> Predicate2 { get; set; }

        public bool Evaluate(V input) {
            return Predicate1.Evaluate(input) && Predicate2.Evaluate(input);
        }
    }

    [DataContract]
    public class OrPredicate<V> : IPredicate<V> {
        [DataMember]
        public IPredicate<V> Predicate1 { get; set; }
        [DataMember]
        public IPredicate<V> Predicate2 { get; set; }

        public bool Evaluate(V input) {
            return Predicate1.Evaluate(input) || Predicate2.Evaluate(input);
        }
    }

    [DataContract]
    public class NotPredicate<V> : IPredicate<V> {
        [DataMember]
        public IPredicate<V> Predicate { get; set; }

        public bool Evaluate(V input) {
            return !Predicate.Evaluate(input);
        }
    }    
}
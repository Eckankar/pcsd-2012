using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeyValueBase.Interfaces;

namespace KeyValueBase {
  public class ValueImpl : IValue {
      public int Value { get; private set; }

      public ValueImpl(int value) {
          this.Value = value;
      }

      public override bool Equals(object obj) {
          return (obj is ValueImpl) && (this.Value == ((ValueImpl)obj).Value);
      }

      public override int GetHashCode() {
          return this.Value.GetHashCode();
      }
  }
}

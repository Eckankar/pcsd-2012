using System;

namespace KeyValueBase.Interfaces
{
  public interface IKey<T> : IComparable<T> 
    where T : IKey<T>
  {
  }
}

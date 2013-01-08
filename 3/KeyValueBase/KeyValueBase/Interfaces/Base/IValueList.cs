using System.Collections.Generic;

namespace KeyValueBase.Interfaces
{
  public interface IValueList<T> : IValue, IEnumerable<T>
  {
    void Add(T item);
    void Remove(T item);
    void Merge(IValueList<T> list);
    IList<T> ToList();
  }
}

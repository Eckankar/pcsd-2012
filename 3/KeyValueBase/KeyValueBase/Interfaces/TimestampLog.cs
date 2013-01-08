using System;
using System.Runtime.Serialization;

namespace KeyValueBase.Interfaces
{
  [DataContract]
  public class TimestampLog : IComparable<TimestampLog>
  {
    [DataMember]
    private long ind;

    public TimestampLog(long ind)
    {
      this.ind = ind;
    }

    public bool IsBefore(TimestampLog t)
    {
      return this.CompareTo(t) < 0;
    }

    public bool IsAfter(TimestampLog t)
    {
      return this.CompareTo(t) > 0;
    }

    public int CompareTo(TimestampLog t)
    {
      return this.ind.CompareTo(t.ind);
    }

    public TimestampLog Increment()
    {
      return new TimestampLog(this.ind + 1);
    }

    public override string ToString()
    {
      return this.ind.ToString();
    }
  }
}

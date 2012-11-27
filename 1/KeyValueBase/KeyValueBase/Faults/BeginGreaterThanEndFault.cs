using System;
using System.Runtime.Serialization;
using KeyValueBase.Interfaces;

namespace KeyValueBase.Faults
{
  [DataContract]
  public class BeginGreaterThanEndFault<K>
    where K : IKey<K>
  {
    [DataMember(Name = "BeginKey")]
    private K beginKey;

    [DataMember(Name = "EndKey")]
    private K endKey;

    [DataMember(Name = "Message")]
    private string message;

    public BeginGreaterThanEndFault(K beginKey, K endKey)
      : this(beginKey, endKey, String.Format("The key '{0}' is greater than '{1}'", beginKey, endKey))
    {
    }

    public BeginGreaterThanEndFault(K beginKey, K endKey, string message)
    {
      this.beginKey = beginKey;
      this.endKey = endKey;
      this.message = message;
    }

    public K BeginKey
    {
      get { return this.beginKey; }
    }
    
    public K EndKey
    {
      get { return this.endKey; }
    }

    public string Message
    {
      get { return this.message; }
    }
  }
}

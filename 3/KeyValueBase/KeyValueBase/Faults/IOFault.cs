using System.Runtime.Serialization;

namespace KeyValueBase.Faults
{
  [DataContract]
  public class IOFault
  {
    [DataMember(Name = "Message")]
    private string message;

    public IOFault()
      : this("I/O error")
    {
    }

    public IOFault(string message)
    {
      this.message = message;
    }

    public string Message
    {
      get { return this.message; }
    }
  }
}

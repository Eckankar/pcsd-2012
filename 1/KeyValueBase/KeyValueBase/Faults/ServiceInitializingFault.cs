using System.Runtime.Serialization;

namespace KeyValueBase.Faults
{
  [DataContract]
  public class ServiceInitializingFault
  {
    [DataMember(Name = "Message")]
    private string message;

    public ServiceInitializingFault()
      : this("Service is being initialized")
    {
    }

    public ServiceInitializingFault(string message)
    {
      this.message = message;
    }

    public string Message
    {
      get { return this.message; }
    }
  }
}

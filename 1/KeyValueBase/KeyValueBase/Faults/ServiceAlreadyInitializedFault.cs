using System.Runtime.Serialization;

namespace KeyValueBase.Faults
{
  [DataContract]
  public class ServiceAlreadyInitializedFault
  {
    [DataMember(Name = "Message")]
    private string message;

    public ServiceAlreadyInitializedFault()
      : this("Service is already initialized")
    {
    }

    public ServiceAlreadyInitializedFault(string message)
    {
      this.message = message;
    }

    public string Message
    {
      get { return this.message; }
    }
  }
}

using System.Runtime.Serialization;

namespace KeyValueBase.Faults
{
  [DataContract]
  public class ServiceNotInitializedFault
  {
    [DataMember(Name = "Message")]
    private string message;

    public ServiceNotInitializedFault()
      : this("Service is not initialized yet")
    {
    }

    public ServiceNotInitializedFault(string message)
    {
      this.message = message;
    }

    public string Message
    {
      get { return this.message; }
    }
  }
}

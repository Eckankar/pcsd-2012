using System.Runtime.Serialization;
using System.ServiceModel;

namespace KeyValueBase.Faults
{
    [DataContract]
    public class ServiceAlreadyConfiguredFault
    {
        [DataMember(Name = "Message")]
        private string message;

        public ServiceAlreadyConfiguredFault()
            : this("The service has already been configured")
        {
        }

        public ServiceAlreadyConfiguredFault(string message)
        {
            this.message = message;
        }

        public string Message
        {
            get { return this.message; }
        }
    }
    public class ServiceAlreadyConfiguredException : FaultException<ServiceAlreadyConfiguredFault>
    {
        private ServiceAlreadyConfiguredException(ServiceAlreadyConfiguredFault fault)
            : base(fault, fault.Message) { }
        public ServiceAlreadyConfiguredException()
            : this(new ServiceAlreadyConfiguredFault()) { }
        public ServiceAlreadyConfiguredException(string message)
            : this(new ServiceAlreadyConfiguredFault(message)) { }
    }
}

using System.Runtime.Serialization;
using System.ServiceModel;

namespace KeyValueBase.Faults {
    [DataContract]
    public class ServiceAlreadyInitializedFault {
        [DataMember(Name = "Message")]
        private string message;

        public ServiceAlreadyInitializedFault()
            : this("Service is already initialized") {
        }

        public ServiceAlreadyInitializedFault(string message) {
            this.message = message;
        }

        public string Message {
            get { return this.message; }
        }
    }

    public class ServiceAlreadyInitializedException : FaultException<ServiceAlreadyInitializedFault> {
        private ServiceAlreadyInitializedException(ServiceAlreadyInitializedFault fault)
            : base(fault, fault.Message) { }
        public ServiceAlreadyInitializedException()
            : this(new ServiceAlreadyInitializedFault()) { }
        public ServiceAlreadyInitializedException(string message)
            : this(new ServiceAlreadyInitializedFault(message)) { }
    }
}

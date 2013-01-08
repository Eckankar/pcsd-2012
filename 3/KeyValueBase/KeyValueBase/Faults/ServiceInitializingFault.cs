using System.Runtime.Serialization;
using System.ServiceModel;

namespace KeyValueBase.Faults {
    [DataContract]
    public class ServiceInitializingFault {
        [DataMember(Name = "Message")]
        private string message;

        public ServiceInitializingFault()
            : this("Service is being initialized") {
        }

        public ServiceInitializingFault(string message) {
            this.message = message;
        }

        public string Message {
            get { return this.message; }
        }
    }

    public class ServiceInitializingException : FaultException<ServiceInitializingFault> {
        private ServiceInitializingException(ServiceInitializingFault fault)
            : base(fault, fault.Message) { }
        public ServiceInitializingException() : this(new ServiceInitializingFault()) { }
        public ServiceInitializingException(string message) : this(new ServiceInitializingFault(message)) { }
    }
}

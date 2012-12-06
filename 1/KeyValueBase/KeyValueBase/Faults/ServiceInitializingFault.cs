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
        public ServiceInitializingException() : base(new ServiceInitializingFault()) { }
        public ServiceInitializingException(string message) : base(new ServiceInitializingFault(message)) { }
    }
}

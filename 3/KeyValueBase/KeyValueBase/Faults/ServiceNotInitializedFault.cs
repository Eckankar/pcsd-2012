using System.Runtime.Serialization;
using System.ServiceModel;

namespace KeyValueBase.Faults {
    [DataContract]
    public class ServiceNotInitializedFault {
        [DataMember(Name = "Message")]
        private string message;

        public ServiceNotInitializedFault()
            : this("Service is not initialized yet") {
        }

        public ServiceNotInitializedFault(string message) {
            this.message = message;
        }

        public string Message {
            get { return this.message; }
        }
    }

    public class ServiceNotInitializedException : FaultException<ServiceNotInitializedFault> {
        private ServiceNotInitializedException(ServiceNotInitializedFault fault)
            : base(fault, fault.Message) { }
        public ServiceNotInitializedException() : this(new ServiceNotInitializedFault()) { }
        public ServiceNotInitializedException(string message) : this(new ServiceNotInitializedFault(message)) { }
    }
}

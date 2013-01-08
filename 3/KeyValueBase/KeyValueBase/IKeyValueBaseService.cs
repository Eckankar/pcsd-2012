using KeyValueBase.Interfaces;
using System.ServiceModel;
using KeyValueBase.Faults;

namespace KeyValueBase {
    [ServiceContract]
    public interface IKeyValueBaseService : IKeyValueBase<KeyImpl, ValueListImpl> {
        [OperationContract]
        [FaultContract(typeof(ServiceInitializingFault))]
        void Reset();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeyValueBase.Interfaces;
using System.ServiceModel;
using KeyValueBase.Faults;
using System.Runtime.Serialization;

namespace KeyValueBase {
    [ServiceContract]
    public interface IKeyValueBaseService : IKeyValueBase<KeyImpl, ValueListImpl> {
        [OperationContract]
        [FaultContract(typeof(ServiceInitializingFault))]
        void Reset();
    }
}

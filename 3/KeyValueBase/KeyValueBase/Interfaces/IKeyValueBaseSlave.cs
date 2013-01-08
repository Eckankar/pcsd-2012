using System.Collections.Generic;
using System.ServiceModel;

namespace KeyValueBase.Interfaces
{
  [ServiceContractAttribute(ConfigurationName = "IKeyValueBaseSlave")]
  public interface IKeyValueBaseSlave<K, V> : IKeyValueBaseReplica<K, V>
    where K : IKey<K>
    where V : IValue
  {
    /// <summary>
    /// Applies a log record
    /// </summary>
    /// <param name="record">a log record</param>
    [OperationContract]
    void LogApply(LogRecord record);
  }
}

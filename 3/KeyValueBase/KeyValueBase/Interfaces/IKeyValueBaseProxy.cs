using System.ServiceModel;
namespace KeyValueBase.Interfaces
{
  //[ServiceContractAttribute(ConfigurationName = "IKeyValueBaseProxy")]
  public interface IKeyValueBaseProxy<K, V> : IKeyValueBase<K, V>
    where K : IKey<K>
    where V : IValue
  {
    /// <summary>
    /// Configures the service
    /// </summary>
    /// <param name="conf">a configuration</param>
    /// <exception cref="ServiceAlreadyConfiguredFault">If the service is already configured</exception>
    //[OperationContract]
    void Configure(Configuration conf);
  }
}

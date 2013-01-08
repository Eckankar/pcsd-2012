using System.Collections.Generic;
using System.ServiceModel;

namespace KeyValueBase.Interfaces
{
  //[ServiceContractAttribute(ConfigurationName = "IKeyValueBaseMaster")]
  public interface IKeyValueBaseMaster<K, V> : IKeyValueBaseReplica<K, V>
    where K : IKey<K>
    where V : IValue
  {
    /// <summary>
    /// Inserts a value and associates it with a specific key
    /// </summary>
    /// <param name="key">a key</param>
    /// <param name="value">a value</param>
    /// <exception cref="KeyAlreadyPresentException">If the key is already present</exception>
    /// <exception cref="ServiceNotInitializedException">If the service is not initialized</exception>
    /// <exception cref="IOException">If an I/O error occurs</exception>
    void Insert(K key, V value);

    /// <summary>
    /// Updates the value associated with a specific key
    /// </summary>
    /// <param name="key">a key</param>
    /// <param name="newValue">a value</param>
    /// <exception cref="KeyNotFoundException">If the requested key is not present</exception>
    /// <exception cref="ServiceNotInitializedException">If the service is not initialized</exception>
    /// <exception cref="IOException">If an I/O error occurs</exception>
    void Update(K key, V newValue);

    /// <summary>
    /// Removes a key and its value
    /// </summary>
    /// <param name="key">a key</param>
    /// <exception cref="KeyNotFoundException">If the requested key is not present</exception>
    /// <exception cref="ServiceNotInitializedException">If the service is not initialized</exception>
    void Delete(K key);

    /// <summary>
    /// Inserts a lsit of key-value pairs at once.
    /// </summary>
    /// <param name="pairs">the list of pairs</param>
    /// <exception cref="ServiceNotInitializedException">If the service is not initialized</exception>
    /// <exception cref="IOException">If an I/O error occurs</exception>
    void BulkPut(IEnumerable<KeyValuePair<K, V>> pairs);

    /// <summary>
    /// Configures the service
    /// </summary>
    /// <param name="conf">a configuration</param>
    /// <exception cref="ServiceAlreadyConfiguredFault">If the service is already configured</exception>
    void Configure(Configuration conf);
  }
}

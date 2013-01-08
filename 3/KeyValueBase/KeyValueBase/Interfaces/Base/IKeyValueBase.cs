using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using KeyValueBase.Faults;

namespace KeyValueBase.Interfaces
{
  [ServiceContract]
  [ServiceKnownType("GetPredicateTypes", typeof(PredicateTypesHelper))]
  public interface IKeyValueBase<K, V>
    where K : IKey<K>
    where V : IValue
  {
    /// <summary>
    /// Initializes the KeyValue store by inserting all pairs from <c>serverFilename</c>
    /// </summary>
    /// <param name="serverFilename">a file name on the server</param>
    /// <exception cref="ServiceAlreadyInitializedException">If the service is already initialized</exception>
    /// <exception cref="ServiceInitializingException">If the service is in process of initializing</exception>
    /// <exception cref="FileNotFoundException">If the provided file is not found</exception>
    [OperationContract]
    [FaultContract(typeof(ServiceAlreadyInitializedFault))]
    [FaultContract(typeof(ServiceInitializingFault))]
    void Init(string serverFilename);

    /// <summary>
    /// Reads the value associated with a key
    /// </summary>
    /// <param name="key">a key</param>
    /// <exception cref="KeyNotFoundException">If the requested key is not present</exception>
    /// <exception cref="ServiceNotInitializedException">If the service is not initialized</exception>
    /// <exception cref="IOException">If an I/O error occurs</exception>
    /// <returns>a value</returns>
    [OperationContract]
    //[FaultContract(typeof(KeyNotFoundFault<K>))]
    [FaultContract(typeof(ServiceNotInitializedFault))]
    V Read(K key);

    /// <summary>
    /// Inserts a value and associates it with a specific key
    /// </summary>
    /// <param name="key">a key</param>
    /// <param name="value">a value</param>
    /// <exception cref="KeyAlreadyPresentException">If the key is already present</exception>
    /// <exception cref="ServiceNotInitializedException">If the service is not initialized</exception>
    /// <exception cref="IOException">If an I/O error occurs</exception>
    [OperationContract]
    //[FaultContract(typeof(KeyAlreadyPresentFault<K>))]
    [FaultContract(typeof(ServiceNotInitializedFault))]
    void Insert(K key, V value);

    /// <summary>
    /// Updates the value associated with a specific key
    /// </summary>
    /// <param name="key">a key</param>
    /// <param name="newValue">a value</param>
    /// <exception cref="KeyNotFoundException">If the requested key is not present</exception>
    /// <exception cref="ServiceNotInitializedException">If the service is not initialized</exception>
    /// <exception cref="IOException">If an I/O error occurs</exception>
    [OperationContract]
    //[FaultContract(typeof(KeyNotFoundFault<K>))]
    [FaultContract(typeof(ServiceNotInitializedFault))]
    void Update(K key, V newValue);

    /// <summary>
    /// Removes a key and its value
    /// </summary>
    /// <param name="key">a key</param>
    /// <exception cref="KeyNotFoundException">If the requested key is not present</exception>
    /// <exception cref="ServiceNotInitializedException">If the service is not initialized</exception>
    [OperationContract]
    //[FaultContract(typeof(KeyNotFoundFault<K>))]
    [FaultContract(typeof(ServiceNotInitializedFault))]
    void Delete(K key);

    /// <summary>
    /// Scans all values associated with keys in the range <c>begin</c> - <c>end</c> and filters 
    /// them using <c>predicate</c>
    /// </summary>
    /// <param name="begin">the start key</param>
    /// <param name="end">the end key</param>
    /// <param name="predicate">the filter</param>
    /// <exception cref="KeyNotFoundException">If the requested key is not present</exception>
    /// <exception cref="ServiceNotInitializedException">If the service is not initialized</exception>
    /// <exception cref="IOException">If an I/O error occurs</exception>
    /// <exception cref="BeginGreaterThanEndException">If the end key is less than the start key</exception>
    /// <returns>a list of value</returns>
    [OperationContract]
    //[FaultContract(typeof(KeyNotFoundFault<K>))]
    [FaultContract(typeof(ServiceNotInitializedFault))]
    //[FaultContract(typeof(BeginGreaterThanEndFault<K>))]
    IEnumerable<V> Scan(K begin, K end, IPredicate<V> predicate);

    /// <summary>
    /// Scans all values associated with keys in the range <c>begin</c> - <c>end</c> and filters 
    /// them using <c>predicate</c>. This operation is atomic.
    /// </summary>
    /// <param name="begin">the start key</param>
    /// <param name="end">the end key</param>
    /// <param name="predicate">the filter</param>
    /// <exception cref="KeyNotFoundException">If the requested key is not present</exception>
    /// <exception cref="ServiceNotInitializedException">If the service is not initialized</exception>
    /// <exception cref="IOException">If an I/O error occurs</exception>
    /// <exception cref="BeginGreaterThanEndException">If the end key is less than the start key</exception>
    /// <returns>a list of value</returns>
    [OperationContract]
    //[FaultContract(typeof(KeyNotFoundFault<K>))]
    [FaultContract(typeof(ServiceNotInitializedFault))]
    //[FaultContract(typeof(BeginGreaterThanEndFault<K>))]
    IEnumerable<V> AtomicScan(K begin, K end, IPredicate<V> predicate);

    /// <summary>
    /// Inserts a lsit of key-value pairs at once.
    /// </summary>
    /// <param name="pairs">the list of pairs</param>
    /// <exception cref="ServiceNotInitializedException">If the service is not initialized</exception>
    /// <exception cref="IOException">If an I/O error occurs</exception>
    [OperationContract]
    [FaultContract(typeof(ServiceNotInitializedFault))]
    void BulkPut(IEnumerable<KeyValuePair<K, V>> pairs);
  }
}

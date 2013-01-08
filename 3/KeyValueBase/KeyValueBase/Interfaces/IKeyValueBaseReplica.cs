using System.Collections.Generic;

namespace KeyValueBase.Interfaces
{
  public interface IKeyValueBaseReplica<K, V>
    where K : IKey<K>
    where V : IValue
  {
    /// <summary>
    /// Initializes the KeyValue store by inserting all pairs from <c>serverFilename</c>
    /// </summary>
    /// <param name="serverFilename">a file name on the server</param>
    /// <exception cref="ServiceAlreadyInitializedFault">If the service is already initialized</exception>
    /// <exception cref="ServiceInitializingFault">If the service is in process of initializing</exception>
    /// <exception cref="FileNotFoundFault">If the provided file is not found</exception>
    void Init(string serverFilename);

    /// <summary>
    /// Reads the value associated with a key
    /// </summary>
    /// <param name="key">a key</param>
    /// <exception cref="KeyNotFoundFault">If the requested key is not present</exception>
    /// <exception cref="ServiceNotInitializedFault">If the service is not initialized</exception>
    /// <exception cref="IOFault">If an I/O error occurs</exception>
    /// <returns>a value with the last timestamp</returns>
    KeyValuePair<TimestampLog, V> Read(K k);

    /// <summary>
    /// Scans all values associated with keys in the range <c>begin</c> - <c>end</c> and filters 
    /// them using <c>predicate</c>
    /// </summary>
    /// <param name="begin">the start key</param>
    /// <param name="end">the end key</param>
    /// <param name="predicate">the filter</param>
    /// <exception cref="KeyNotFoundFault">If the requested key is not present</exception>
    /// <exception cref="ServiceNotInitializedFault">If the service is not initialized</exception>
    /// <exception cref="IOFault">If an I/O error occurs</exception>
    /// <exception cref="BeginGreaterThanEndFault">If the end key is less than the start key</exception>
    /// <returns>a list of values with the last timestamp</returns>
    KeyValuePair<TimestampLog, IEnumerable<V>> Scan(K begin, K end, IPredicate<V> p);

    /// <summary>
    /// Scans all values associated with keys in the range <c>begin</c> - <c>end</c> and filters 
    /// them using <c>predicate</c>. This method is atomic.
    /// </summary>
    /// <param name="begin">the start key</param>
    /// <param name="end">the end key</param>
    /// <param name="predicate">the filter</param>
    /// <exception cref="KeyNotFoundFault">If the requested key is not present</exception>
    /// <exception cref="ServiceNotInitializedFault">If the service is not initialized</exception>
    /// <exception cref="IOFault">If an I/O error occurs</exception>
    /// <exception cref="BeginGreaterThanEndFault">If the end key is less than the start key</exception>
    /// <returns>a list of values with the last timestamp</returns>
    KeyValuePair<TimestampLog, IEnumerable<V>> AtomicScan(K begin, K end, IPredicate<V> p);
  }
}

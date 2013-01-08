using System.Collections.Generic;

namespace KeyValueBase.Interfaces
{
  public interface IIndex<K, V> 
    where K : IKey<K>
    where V : IValue
  {
    /// <summary>
    /// Inserts a new key associated with a value
    /// </summary>
    /// <param name="key">a key</param>
    /// <param name="values">a value</param>
    /// <exception cref="KeyAlreadyPresentException">The key is already present</exception>
    /// <exception cref="IOException">If an I/O error occurs</exception>
    void Insert(K key, V value);

    /// <summary>
    /// Removes a key and the value associated with it
    /// </summary>
    /// <exception cref="KeyNotFoundException">If the key is not present</exception>
    /// <param name="key">a key</param>
    void Remove(K key);

    /// <summary>
    /// Updates the value associated with a key
    /// </summary>
    /// <param name="key">a key</param>
    /// <param name="newValue">a value</param>
    /// <exception cref="KeyNotFoundException">If the key is not present</exception>
    /// <exception cref="IOException">If an I/O error occurs</exception>
    void Update(K key, V newValue);

    /// <summary>
    /// Gets the value associated with a key
    /// </summary>
    /// <param name="key">a key</param>
    /// <exception cref="KeyNotFoundException">If the key is not present</exception>
    /// <exception cref="IOException">If an I/O error occurs</exception>
    /// <returns>a value</returns>
    V Get(K key);

    /// <summary>
    /// Scans all values between a range of keys
    /// </summary>
    /// <param name="begin">the start key</param>
    /// <param name="end">the end key</param>
    /// <exception cref="BeginGreaterThanEndException">If the end key is less than the start key</exception>
    /// <exception cref="IOException">If an I/O error occurs</exception>
    /// <returns>list of values</returns>
    IEnumerable<V> Scan(K begin, K end);

    /// <summary>
    /// Scans all values between a range of keys. This operation is atomic.
    /// </summary>
    /// <param name="begin">the start key</param>
    /// <param name="end">the end key</param>
    /// <exception cref="BeginGreaterThanEndException">If the end key is less than the start key</exception>
    /// <exception cref="IOException">If an I/O error occurs</exception>
    /// <returns>list of values</returns>
    IEnumerable<V> AtomicScan(K begin, K end);

    /// <summary>
    /// Isnerts a batch of key-value pairs.
    /// </summary>
    /// <exception cref="IOException">If an I/O error occurs</exception>
    /// <param name="pairs">a list of pairs</param>
    void BulkPut(IEnumerable<KeyValuePair<K, V>> pairs);
  }
}

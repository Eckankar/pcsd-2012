namespace KeyValueBase.Interfaces
{
  public interface IValueSerializer<V> 
    where V : IValue
  {
    /// <summary>
    /// Creates a value from a byte array
    /// </summary>
    /// <param name="array">the byte array</param>
    /// <returns>a value</returns>
    V FromByteArray(byte[] array);

    /// <summary>
    /// Creates a byte array from a value
    /// </summary>
    /// <param name="value">the value</param>
    /// <returns>a byte array</returns>
    byte[] ToByteArray(V value);
  }
}

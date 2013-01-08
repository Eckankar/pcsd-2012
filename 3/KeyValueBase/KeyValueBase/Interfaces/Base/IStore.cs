namespace KeyValueBase.Interfaces
{
  public interface IStore
  {
    byte[] Read(long position, int length);
    void Write(long position, byte[] value);
  }
}

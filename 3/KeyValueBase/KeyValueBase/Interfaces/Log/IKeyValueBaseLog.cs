namespace KeyValueBase.Interfaces
{
  public interface IKeyValueBaseLog<K, V>
    where K : IKey<K>
    where V : IValue
  {
    void Quiesce();
    void Resume();
  }
}

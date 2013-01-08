using System;
using KeyValueBase.Interfaces;
using System.Linq;

namespace KeyValueBase
{
  public class KeyValueBaseSlaveImpl : KeyValueBaseReplicaImpl, IKeyValueBaseSlave<KeyImpl, ValueListImpl>
  {
    public void LogApply(LogRecord record)
    {
        stamp = record.Lsn;
        KeyImpl k = new KeyImpl((int)record.Parameters[0]);
        ValueListImpl v = new ValueListImpl();
        foreach (object p in record.Parameters.Skip(1))
        {
            v.Add((ValueImpl)p);
        }
        switch (record.OperationType)
        {
            case OperationType.Checkpoint:
                // Durability ignored
                break;
            case OperationType.Delete:
                index.Remove(k);
                break;
            case OperationType.Insert:
                index.Insert(k, v);
                break;
            case OperationType.Update:
                index.Update(k, v);
                break;
        }
    }
  }
}

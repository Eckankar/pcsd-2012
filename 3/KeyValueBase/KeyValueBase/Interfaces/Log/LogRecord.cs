using System;
using System.Runtime.Serialization;

namespace KeyValueBase.Interfaces
{
  public enum OperationType
  {
    Update,
    Insert,
    Delete,
    Checkpoint
  }

  /// <summary>
  /// Make sure that all parameters are serializable
  /// </summary>
  [DataContract]
  public class LogRecord
  {
    private static TimestampLog lastTimestamp = new TimestampLog(0L);

    [DataMember]
    private OperationType operationType;
    [DataMember]
    private TimestampLog lsn;
    [DataMember]
    private object[] parameters;

    public LogRecord(OperationType operationType, object[] parameters)
    {
      this.operationType = operationType;
      this.parameters = parameters;
      
      lock (LogRecord.lastTimestamp)
      {
        this.lsn = LogRecord.lastTimestamp.Increment();
      }
    }

    public TimestampLog Lsn
    {
      get { return this.lsn; }
    }

    public OperationType OperationType
    {
      get { return this.operationType; }
    }

    public object[] Parameters
    {
      get { return this.parameters; }
    }
  }
}

using System;

namespace KeyValueBase.Interfaces
{
  //
  // The logger uses c# Async pattern
  // http://msdn.microsoft.com/en-us/library/ms228963.aspx
  public interface IReplicator
  {
    IAsyncResult BeginMakeStable(LogRecord record, AsyncCallback callback, object asyncState);
    void EndMakeStable(IAsyncResult asyncResult);
  }
}

using System;

namespace KeyValueBase.Interfaces
{
  //
  // The logger uses c# Async pattern
  // http://msdn.microsoft.com/en-us/library/ms228963.aspx
  public interface ILogger
  {
    IAsyncResult BeginLogRequest(LogRecord record, AsyncCallback callback, object asyncState);
    void EndLogRequest(IAsyncResult asyncResult);
  }
}

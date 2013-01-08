using System;
using System.Threading;

namespace KeyValueBase.Interfaces
{
  public class ReplicationAsyncResult : IAsyncResult
  {
    private readonly ManualResetEvent resetEvent;

    public ReplicationAsyncResult()
      : this(false, null)
    {
    }

    public ReplicationAsyncResult(object asyncState)
      : this(false, asyncState)
    {
    }

    public ReplicationAsyncResult(bool completedSynchronously, object asyncState)
    {
      this.resetEvent = new ManualResetEvent(completedSynchronously);
      this.CompletedSynchronously = completedSynchronously;
      this.IsCompleted = completedSynchronously;
      this.AsyncState = asyncState;
    }

    public object AsyncState
    {
      get;
      set;
    }

    public WaitHandle AsyncWaitHandle
    {
      get { return this.resetEvent; }
    }

    public bool CompletedSynchronously
    {
      get;
      private set;
    }

    public bool IsCompleted
    {
      get;
      private set;
    }

    public void Complete()
    {
      if (!IsCompleted)
      {
        // mark as completed
        IsCompleted = true;

        // notify all waiting threads
        this.resetEvent.Set();
      }
    }
  }
}

using System;
using KeyValueBase.Interfaces;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace KeyValueBase
{
    public class ReplicatorImpl : IReplicator
    {
        private List<IKeyValueBaseSlave<KeyImpl, ValueListImpl>> clients = new List<IKeyValueBaseSlave<KeyImpl, ValueListImpl>>();

        public void Configure(Interfaces.Configuration c)
        {
            foreach (Uri uri in c.SlaveWsdlUrls)
            {
                clients.Add(new KeyValueBaseSlaveClient(uri));
            }
        }

        private void _makeStable(LogRecord record)
        {
            foreach (IKeyValueBaseSlave<KeyImpl, ValueListImpl> c in clients)
            {
                c.LogApply(record);
            }
        }

        private delegate void myDelegate(LogRecord record);


        public IAsyncResult BeginMakeStable(LogRecord record, AsyncCallback callback, object asyncState)
        {
            myDelegate del = new myDelegate(_makeStable);
            return del.BeginInvoke(record, callback, asyncState);
        }

        public void EndMakeStable(IAsyncResult asyncResult)
        {
            AsyncResult r = (AsyncResult)asyncResult;
            myDelegate lw = (myDelegate)r.AsyncDelegate;
            lw.EndInvoke(asyncResult);
        }
    }
}

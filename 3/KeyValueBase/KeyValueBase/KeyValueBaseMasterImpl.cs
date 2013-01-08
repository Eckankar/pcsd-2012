using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeyValueBase.Interfaces;
using KeyValueBase.Faults;
using System.ServiceModel;

namespace KeyValueBase
{
    //[ServiceContract]
    public class KeyValueBaseMasterImpl : KeyValueBaseReplicaImpl, IKeyValueBaseMaster<KeyImpl, ValueListImpl>
    {
        private Configuration configuration;
        private ReplicatorImpl replicator = new ReplicatorImpl();

        public void Insert(KeyImpl key, ValueListImpl value)
        {
            List<Object> param = new List<Object>(value.ToList());
            param.Insert(0, key);
            LogRecord record = new LogRecord(OperationType.Insert, param.ToArray());
            IAsyncResult ar = replicator.BeginMakeStable(record, null, null);
            index.Insert(key, value);
            replicator.EndMakeStable(ar);
        }

        public void Update(KeyImpl key, ValueListImpl value)
        {
            List<Object> param = new List<Object>(value.ToList());
            param.Insert(0, key);
            LogRecord record = new LogRecord(OperationType.Update, param.ToArray());
            IAsyncResult ar;
            stamp = record.Lsn;
            ar = replicator.BeginMakeStable(record, null, null);
            index.Update(key, value);
            replicator.EndMakeStable(ar);
        }

        public void Delete(KeyImpl key)
        {
            object[] k = new object[1];
            k[0] = key.Key;
            LogRecord record = new LogRecord(OperationType.Delete, k);
            IAsyncResult ar = replicator.BeginMakeStable(record, null, null);
            index.Remove(key);
            stamp = record.Lsn;
            replicator.EndMakeStable(ar);
        }

        public void BulkPut(IEnumerable<KeyValuePair<KeyImpl, ValueListImpl>> pairs)
        {
            List<LogRecord> records = new List<LogRecord>();
            List<IAsyncResult> handles = new List<IAsyncResult>();
            foreach (KeyValuePair<KeyImpl, ValueListImpl> p in pairs)
            {
                List<Object> param = new List<Object>(p.Value.ToList());
                param.Insert(0, p.Key);
                records.Add(new LogRecord(OperationType.Insert, param.ToArray()));
            }
            // First update the master, making the others stale!
            index.BulkPut(pairs);
            stamp = records.Last().Lsn;
            // Then propagate
            foreach (LogRecord r in records)
            {
                handles.Add(replicator.BeginMakeStable(r, null, null));
            }
            foreach (IAsyncResult i in handles)
            {
                replicator.EndMakeStable(i);
            }
        }

        public void Configure(Configuration conf)
        {
            if (configuration == null)
            {
                configuration = conf;
                replicator.Configure(conf);
            }
            else
                throw new ServiceAlreadyConfiguredException();
        }
    }
}

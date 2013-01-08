using System;
using System.Collections.Generic;
using KeyValueBase.Interfaces;
using System.Linq;

namespace KeyValueBase
{
    public class KeyValueBaseProxyImpl : IKeyValueBaseProxy<KeyImpl, ValueListImpl>
    {
        private List<KeyValueBaseSlaveClient> slaves = new List<KeyValueBaseSlaveClient>();
        private KeyValueBaseMasterClient master;
        private bool masterIsLive;
        private TimestampLog LastLSN;

        public void Configure(Interfaces.Configuration c)
        {
            foreach (Uri uri in c.SlaveWsdlUrls)
            {
                slaves.Add(new KeyValueBaseSlaveClient(uri));
            }
            master = new KeyValueBaseMasterClient(c.MasterWsdlUrl);
            masterIsLive = true;
        }

        public void Init(string serverFilename)
        {
            master.Init(serverFilename);
            foreach (KeyValueBaseSlaveClient s in slaves.OrderByDescending(a => Guid.NewGuid()))
            {
                s.Init(serverFilename);
            }
        }

        public ValueListImpl Read(KeyImpl key)
        {
            KeyValuePair<TimestampLog, ValueListImpl> val;
            Boolean retry = true;
            foreach (KeyValueBaseSlaveClient s in slaves.OrderByDescending(a => Guid.NewGuid()))
            {
                val = s.Read(key);
                if (LastLSN.IsBefore(val.Key))
                {
                    LastLSN = val.Key;
                    retry = false;
                    break;
                }
            }
            if (retry && masterIsLive) try
                {
                    val = master.Read(key);
                }
                catch (TimeoutException)
                {
                    masterIsLive = false;
                }

            return val.Value;
        }

        public void Insert(KeyImpl key, ValueListImpl value)
        {
            try
            {
                if (masterIsLive)
                    master.Insert(key, value);
            }
            catch (TimeoutException)
            {
                masterIsLive = false;
            }
        }

        public void Update(KeyImpl key, ValueListImpl newValue)
        {
            try
            {
                if (masterIsLive)
                    master.Update(key, newValue);
            }
            catch (TimeoutException)
            {
                masterIsLive = false;
            }
        }

        public void Delete(KeyImpl key)
        {
            try
            {
                if (masterIsLive)
                    master.Delete(key);
            }
            catch (TimeoutException)
            {
                masterIsLive = false;
            }
        }

        public IEnumerable<ValueListImpl> Scan(KeyImpl begin, KeyImpl end, IPredicate<ValueListImpl> predicate)
        {
            KeyValuePair<TimestampLog, IEnumerable<ValueListImpl>> val;
            Boolean retry = true;
            foreach (KeyValueBaseSlaveClient s in slaves.OrderByDescending(a => Guid.NewGuid()))
            {
                try
                {
                    val = s.Scan(begin, end, predicate);
                }
                catch (TimeoutException)
                {
                    slaves.Remove(s); // Remove slave on timeout
                }

                if (LastLSN.IsBefore(val.Key))
                {
                    LastLSN = val.Key;
                    retry = false;
                    break;
                }
            }
            if (retry)
                val = master.Scan(begin, end, predicate);

            return val.Value;
        }

        public IEnumerable<ValueListImpl> AtomicScan(KeyImpl begin, KeyImpl end, IPredicate<ValueListImpl> predicate)
        {
            KeyValuePair<TimestampLog, IEnumerable<ValueListImpl>> val;
            Boolean retry = true;
            foreach (KeyValueBaseSlaveClient s in slaves.OrderByDescending(a => Guid.NewGuid()))
            {
                try
                {
                    val = s.AtomicScan(begin, end, predicate);
                }
                catch (TimeoutException)
                {
                    slaves.Remove(s);
                }

                if (LastLSN.IsBefore(val.Key))
                {
                    LastLSN = val.Key;
                    retry = false;
                    break;
                }
            }
            if (retry && masterIsLive) try
                {
                    val = master.AtomicScan(begin, end, predicate);
                }
                catch (TimeoutException)
                {
                    masterIsLive = false;
                }

            return val.Value;
        }

        public void BulkPut(IEnumerable<KeyValuePair<KeyImpl, ValueListImpl>> pairs)
        {
            try
            {
                if (masterIsLive)
                    master.BulkPut(pairs);
            }
            catch (TimeoutException)
            {
                masterIsLive = false;
            }
        }
    }
}

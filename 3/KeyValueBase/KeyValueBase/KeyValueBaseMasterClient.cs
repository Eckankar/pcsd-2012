using KeyValueBase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace KeyValueBase
{
    public class KeyValueBaseMasterClient : IKeyValueBaseMaster<KeyImpl, ValueListImpl>
    {
        public IKeyValueBaseMaster<KeyImpl, ValueListImpl> service;

        public KeyValueBaseMasterClient(Uri uri)
        {
            // Any channel setup code goes here
            EndpointAddress address = new EndpointAddress(uri);
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.Transport);
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxBufferSize = 65536;
            binding.MaxReceivedMessageSize = 104857600;

            ChannelFactory<IKeyValueBaseMaster<KeyImpl, ValueListImpl>> factory = new ChannelFactory<IKeyValueBaseMaster<KeyImpl, ValueListImpl>>(binding, address);
            service = factory.CreateChannel();
        }

        public void Init(string serverFilename)
        {
            service.Init(serverFilename);
        }

        public KeyValuePair<TimestampLog, ValueListImpl> Read(KeyImpl k)
        {
            return service.Read(k);
        }

        public KeyValuePair<TimestampLog, IEnumerable<ValueListImpl>> Scan(KeyImpl begin, KeyImpl end, IPredicate<ValueListImpl> p)
        {
            return service.Scan(begin, end, p);
        }

        public KeyValuePair<TimestampLog, IEnumerable<ValueListImpl>> AtomicScan(KeyImpl begin, KeyImpl end, IPredicate<ValueListImpl> p)
        {
            return service.AtomicScan(begin, end, p);
        }

        public void Insert(KeyImpl key, ValueListImpl value)
        {
            service.Insert(key, value);
        }

        public void Update(KeyImpl key, ValueListImpl newValue)
        {
            service.Update(key, newValue);
        }

        public void Delete(KeyImpl key)
        {
            service.Delete(key);
        }

        public void BulkPut(IEnumerable<KeyValuePair<KeyImpl, ValueListImpl>> pairs)
        {
            service.BulkPut(pairs);
        }

        public void Configure(Configuration conf)
        {
            service.Configure(conf);
        }
    }
}
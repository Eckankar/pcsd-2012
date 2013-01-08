using KeyValueBase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace KeyValueBase
{
    public class KeyValueBaseSlaveClient : IKeyValueBaseSlave<KeyImpl, ValueListImpl>
    {
        public IKeyValueBaseSlave<KeyImpl, ValueListImpl> service;

        public KeyValueBaseSlaveClient(Uri uri)
        {
            // Any channel setup code goes here
            EndpointAddress address = new EndpointAddress(uri);
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.Transport);
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxBufferSize = 65536;
            binding.MaxReceivedMessageSize = 104857600;

            ChannelFactory<IKeyValueBaseSlave<KeyImpl, ValueListImpl>> factory = new ChannelFactory<IKeyValueBaseSlave<KeyImpl, ValueListImpl>>(binding, address);
            service = factory.CreateChannel();
        }


        public void LogApply(LogRecord record)
        {
            service.LogApply(record);
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
    }
}
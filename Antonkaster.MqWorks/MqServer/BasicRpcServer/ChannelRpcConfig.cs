using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace Antonkaster.MqWorks.MqServer.BasicRpcServer
{
    internal class ChannelRpcConfig : ChannelConfig
    {
        public Func<byte[],byte[]> OnRecieveRpcFunction { get; set; }
    }
}

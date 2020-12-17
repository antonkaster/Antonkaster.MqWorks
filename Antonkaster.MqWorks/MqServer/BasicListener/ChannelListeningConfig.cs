using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace Antonkaster.MqWorks.MqServer.BasicListener
{
    internal class ChannelListeningConfig : ChannelConfig
    {
        public Action<byte[]> OnRecieveAction { get; set; }
    }
}

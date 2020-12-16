using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace Antonkaster.MqWorks.MqServer
{
    internal class ChannelItem
    {
        public string ConsumerTag { get; set; }
        public IModel Channel { get; set; }
        public EventingBasicConsumer Consumer { get; set; }
        public Action<byte[]> OnRecieveAction { get; set; }
    }
}

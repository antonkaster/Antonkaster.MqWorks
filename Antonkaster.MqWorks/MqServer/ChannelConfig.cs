using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace Antonkaster.MqWorks.MqServer
{
    internal class ChannelConfig
    {
        public string ConsumerTag { get; set; }
        public string ChannelName { get; set; }
        public IModel Channel { get; set; }
        public EventingBasicConsumer Consumer { get; set; }
    }
}

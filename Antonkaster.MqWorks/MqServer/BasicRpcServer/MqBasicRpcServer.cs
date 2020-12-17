using Antonkaster.MqWorks.Exceptions;
using Antonkaster.MqWorks.Extensions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Antonkaster.MqWorks.MqServer.BasicRpcServer
{
    public class MqBasicRpcServer : MqBase
    {
        private readonly Dictionary<string, ChannelRpcConfig> channels = new Dictionary<string, ChannelRpcConfig>();

        public MqBasicRpcServer(IMqConnectionConfig connectionConfig) : base(connectionConfig)
        {
        }

        public MqBasicRpcServer(ConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public MqBasicRpcServer(IConnection connection) : base(connection)
        {
        }


        public MqBasicRpcServer StartListening(string channelName, Func<byte[],byte[]> onRecieveFunction, IBasicProperties properties = null)
        {
            if (string.IsNullOrWhiteSpace(channelName))
                throw new ArgumentNullException("Channe name can't be null!");
            if (onRecieveFunction == null)
                throw new ArgumentNullException("OnRecieve action can't be null!");

            IModel channel = Connection.CreateModel();
            channel.QueueDeclare(channelName, true, false, false, null);
            channel.BasicQos(0, 1, false);

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;

            string consumerTag = channel.BasicConsume(channelName, false, consumer);

            channels.Add(
                consumerTag,
                new ChannelRpcConfig()
                {
                    ConsumerTag = consumerTag,
                    Channel = channel,
                    Consumer = consumer,
                    ChannelName = channelName,
                    OnRecieveRpcFunction = onRecieveFunction
                });

            return this;
        }

        public MqBasicRpcServer StartListening<TRequest,TResult>(string channelName, Func<TRequest, TResult> onRecieveFunction, IBasicProperties properties = null)
        {
            if (onRecieveFunction == null)
                throw new ArgumentNullException("OnRecieve function can't be null!");

            return StartListening(channelName, m => onRecieveFunction.Invoke(m.ToObject<TRequest>()).ToBytes(), properties);
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            ChannelRpcConfig channelItem = channels[e.ConsumerTag];

            if (channelItem == null)
                throw new MqRecieveException($"Channel not found (consumerTag: {e.ConsumerTag})!");

            try
            {
                byte[] result = channelItem.OnRecieveRpcFunction.Invoke(e.Body.ToArray());

                IBasicProperties props = channelItem.Channel.CreateBasicProperties();
                props.CorrelationId = e.BasicProperties.CorrelationId;

                channelItem.Channel.BasicPublish("", e.BasicProperties.ReplyTo, props, result);
                channelItem.Channel.BasicAck(e.DeliveryTag, false);

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
        }

        public void StopListening()
        {
            foreach (ChannelRpcConfig channel in channels.Values)
                channel.Channel.Close();
        }

        protected override void DisposeManagedResources()
        {
            StopListening();

            foreach (ChannelRpcConfig channel in channels.Values)
                channel.Channel.Dispose();

            base.DisposeManagedResources();
        }
    }
}

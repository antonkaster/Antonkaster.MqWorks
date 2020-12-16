using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Antonkaster.MqWorks.MqServer
{
    public class MqBasicListener : MqBase
    {
        public bool AutoAckMessages { get; set; } = false;

        private readonly Dictionary<string, ChannelItem> channels = new Dictionary<string, ChannelItem>();

        public MqBasicListener(MqConnectionConfig connectionConfig) : base(connectionConfig)
        {
        }

        public MqBasicListener(ConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public MqBasicListener(IConnection connection) : base(connection)
        {
        }

        public void StartListening(string channelName, Action<byte[]> onRecieveAction, IBasicProperties properties = null)
        {
            if (string.IsNullOrWhiteSpace(channelName))
                throw new ArgumentNullException("Channe name can't be null!");
            if (onRecieveAction == null)
                throw new ArgumentNullException("OnRecieve action can't be null!");

            IModel channel = Connection.CreateModel();
            channel.QueueDeclare(channelName, true, false, false, null);

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;

            string consumerTag = channel.BasicConsume(channelName, AutoAckMessages, consumer);

            channels.Add(
                consumerTag, 
                new ChannelItem() 
                { 
                    ConsumerTag = consumerTag, 
                    Channel = channel, 
                    Consumer = consumer,
                    OnRecieveAction = onRecieveAction
                });

        }

        public void StartListening(string channelName, Action<string> onRecieveAction, IBasicProperties properties = null)
        {
            if (onRecieveAction == null)
                throw new ArgumentNullException("OnRecieve action can't be null!");
            StartListening(channelName, m => onRecieveAction.Invoke(Encoding.UTF8.GetString(m)), properties);
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            ChannelItem channelItem = channels[e.ConsumerTag];

            try
            {
                channelItem.OnRecieveAction?.Invoke(e.Body.ToArray());

                if (!AutoAckMessages)
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
            foreach(ChannelItem channel in channels.Values)
                channel.Channel.Close();
        }

        protected override void DisposeManagedResources()
        {
            StopListening();

            foreach (ChannelItem channel in channels.Values)
                channel.Channel.Dispose();

            base.DisposeManagedResources();
        }

    }
}

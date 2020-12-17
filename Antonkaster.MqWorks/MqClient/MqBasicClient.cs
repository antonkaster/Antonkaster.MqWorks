using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antonkaster.MqWorks.Extensions;

namespace Antonkaster.MqWorks.MqClient
{
    public class MqBasicClient : MqBase
    {
        public MqBasicClient(IMqConnectionConfig connectionConfig) : base(connectionConfig)
        {
        }

        public MqBasicClient(ConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public MqBasicClient(IConnection connection) : base(connection)
        {
        }

        public MqBasicClient SendToChannel(
            byte[] bytesToSend, 
            string channelName, 
            IBasicProperties properties = null, 
            bool autoDelete = false)
        {
            using IModel channel = Connection.CreateModel();

            channel.QueueDeclare(channelName, true, false, autoDelete, null);
            channel.BasicPublish("", channelName, properties, bytesToSend);

            return this;
        }

        public MqBasicClient SendToChannel<TSend>(TSend objToSend, string channelName, IBasicProperties properties = null, bool autoDelete = false)
            => SendToChannel(objToSend.ToBytes(), channelName, properties, autoDelete);

    }
}

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
        public MqBasicClient(MqConnectionConfig connectionConfig) : base(connectionConfig)
        {
        }

        public MqBasicClient(ConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public MqBasicClient(IConnection connection) : base(connection)
        {
        }

        public void SendToChannel(byte[] bytesToSend, string channelName, IBasicProperties properties = null)
        {
            using IModel channel = Connection.CreateModel();

            channel.QueueDeclare(channelName, true, false, false, null);
            channel.BasicPublish("", channelName, properties, bytesToSend);
        }

        public void SendToChannel(string stringToSend, string channelName, IBasicProperties properties = null)
            => SendToChannel(Encoding.UTF8.GetBytes(stringToSend), channelName, properties);

        public void SendToChannel<TSend>(TSend objToSend, string channelName, IBasicProperties properties = null)
            => SendToChannel(objToSend.ToBytes(), channelName, properties);

    }
}

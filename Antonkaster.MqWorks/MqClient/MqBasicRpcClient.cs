using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antonkaster.MqWorks.Extensions;

namespace Antonkaster.MqWorks.MqClient
{
    public class MqBasicRpcClient : MqBase
    {
        public MqBasicRpcClient(MqConnectionConfig connectionConfig) : base(connectionConfig)
        {
        }

        public MqBasicRpcClient(ConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public MqBasicRpcClient(IConnection connection) : base(connection)
        {
        }

        public TResponse CallRemoteProcedure<TResponse>(byte[] requestBytes, string channelName)
        {
            if (requestBytes == null)
                throw new ArgumentNullException("Request bytes can't be null!");

            using (IModel channel = Connection.CreateModel())
            {
                string replyQueueName = channel.QueueDeclare().QueueName;
                EventingBasicConsumer _consumer = new EventingBasicConsumer(channel);

                string correlationId = Guid.NewGuid().ToString();

                IBasicProperties props = channel.CreateBasicProperties();
                props.CorrelationId = correlationId;
                props.ReplyTo = replyQueueName;

                BlockingCollection<byte[]> respQueue = new BlockingCollection<byte[]>();

                _consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    if (ea.BasicProperties.CorrelationId == correlationId)
                    {
                        respQueue.Add(body);
                    }
                };

                channel.BasicPublish("", channelName, props, requestBytes);
                channel.BasicConsume(_consumer, replyQueueName, true);

                byte[] data = respQueue.Take();

                return data.ToObject<TResponse>();
            }
        }

        public TResponse CallRemoteProcedure<TResponse>(string requestString, string channelName)
            => CallRemoteProcedure<TResponse>(Encoding.UTF8.GetBytes(requestString), channelName);

        public TResponse CallRemoteProcedure<TResponse, TRequest>(TRequest requestData, string channelName)
            => CallRemoteProcedure<TResponse>(requestData.ToBytes(), channelName);
    }
}

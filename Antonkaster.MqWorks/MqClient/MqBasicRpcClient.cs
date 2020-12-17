﻿using RabbitMQ.Client;
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
        /// <summary>
        /// Request timeout (msec)
        /// 0 = infinity
        /// </summary>
        public int Timeout { get; set; } = 0;

        public MqBasicRpcClient(IMqConnectionConfig connectionConfig) : base(connectionConfig)
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
                    if (ea.BasicProperties.CorrelationId == correlationId)
                        respQueue.Add(ea.Body.ToArray());
                };
                

                channel.BasicPublish("", channelName, props, requestBytes);
                channel.BasicConsume(_consumer, replyQueueName, true);

                byte[] data = new byte[0];

                if (Timeout == 0)
                {
                    data = respQueue.Take();
                }
                else
                {
                    DateTime started = DateTime.Now;
                    DateTime expired = started.AddMilliseconds(Timeout);

                    while(!respQueue.TryTake(out data))
                    {
                        if (DateTime.Now > expired)
                        {
                            channel.Abort();
                            channel.Close();
                            throw new MqRpcResponseTimeout();
                        }
                    }

                }
                return data.ToObject<TResponse>();
            }
        }

        public TResponse CallRemoteProcedure<TRequest, TResponse>(TRequest requestData, string channelName)
            => CallRemoteProcedure<TResponse>(requestData.ToBytes(), channelName);
    }
}

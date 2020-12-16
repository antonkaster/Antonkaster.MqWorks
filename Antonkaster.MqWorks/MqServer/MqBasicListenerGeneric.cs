using Antonkaster.MqWorks.Extensions;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Antonkaster.MqWorks.MqServer
{
    public class MqBasicListener<T> : MqBasicListener
    {
        public MqBasicListener(MqConnectionConfig connectionConfig) : base(connectionConfig)
        {
        }

        public MqBasicListener(ConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public MqBasicListener(IConnection connection) : base(connection)
        {
        }

        public void StartListening(string channelName, Action<T> onRecieveAction, IBasicProperties properties = null)
        {
            if (onRecieveAction == null)
                throw new ArgumentNullException("OnRecieve action can't be null!");

            base.StartListening(channelName, m => onRecieveAction.Invoke(m.ToObject<T>()), properties);
        }
    }
}

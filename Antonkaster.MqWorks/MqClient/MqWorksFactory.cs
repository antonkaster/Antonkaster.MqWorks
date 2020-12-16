using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonkaster.MqWorks.MqClient
{
    public static class MqWorksFactory
    {
        public static ConnectionFactory CreateConnectionFactory(MqConnectionConfig config)
        {
            return new ConnectionFactory()
            {
                HostName = config.Host,
                VirtualHost = config.VHost,
                UserName = config.User,
                Password = config.Password
            };
        }
    }
}

using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Antonkaster.MqWorks
{
    public abstract class MqBase : IDisposable
    {
        private IConnection _connection = null;
        public IConnection Connection
        {
            get
            {
                if (_connection != null)
                    return _connection;

                if (_connectionFactory != null)
                {
                    _connection = _connectionFactory.CreateConnection();
                    return _connection;
                }

                throw new InvalidOperationException("Can't create connection!");

            }
            set
            {
                _connection = value;
            }
        }

        public bool CloseConnectionOnDispose { get; set; } = false;

        private readonly ConnectionFactory _connectionFactory = null;

        public MqBase(MqConnectionConfig connectionConfig)
        {
            if (connectionConfig == null)
                throw new ArgumentNullException("Connection config can't be null!");

            _connectionFactory = new ConnectionFactory()
            {
                HostName = connectionConfig.Host,
                VirtualHost = connectionConfig.VHost,
                UserName = connectionConfig.User,
                Password = connectionConfig.Password
            };
            CloseConnectionOnDispose = true;
        }

        public MqBase(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException("Connection facrory can't be null!");
            CloseConnectionOnDispose = true;
        }

        public MqBase(IConnection connection)
        {
            Connection = connection ?? throw new ArgumentNullException("Connection can't be null!");
            CloseConnectionOnDispose = false;
        }


        public void Dispose()
        {
            DisposeManagedResources();
        }

        protected virtual void DisposeManagedResources()
        {
            if (CloseConnectionOnDispose)
            {
                Connection?.Close();
                Connection?.Dispose();
            }
        }
    }
}

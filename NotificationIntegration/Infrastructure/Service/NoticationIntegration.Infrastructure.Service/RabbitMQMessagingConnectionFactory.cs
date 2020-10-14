using System;
using RabbitMQ.Client;

namespace NoticationIntegration.Infrastructure.Service
{
    public class RabbitMQMessagingConnectionFactory
    {
        private ConnectionFactory connectionFactory;

        private IConnection connection;

        private readonly object connectionLockObject = new object();

        public RabbitMQMessagingConnectionFactory(string connectionString)
        {
            Initialize(connectionString);
        }

        private void Initialize(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "Connection String cannot be empty or null.");
            }

            var rabbitMqConnectionUri = new Uri(connectionString);

            connectionFactory = new ConnectionFactory
            {
                Uri = rabbitMqConnectionUri
            };

            if (rabbitMqConnectionUri.Scheme == "amqps")
            {
                connectionFactory.Ssl = new SslOption
                {
                    Enabled = true,
                    Version = System.Security.Authentication.SslProtocols.Tls12,
                    ServerName = connectionFactory.HostName
                };
            }
        }

        public IConnection Get()
        {
            lock (connectionLockObject)
            {
                if (null == connection || !connection.IsOpen)
                {
                    connection = connectionFactory.CreateConnection();
                }
            }

            return connection;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (connectionLockObject)
                {
                    connection?.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

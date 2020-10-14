using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NotificationIntegration.Domain.Services.Interfaces;
using RabbitMQ.Client;

namespace NoticationIntegration.Infrastructure.Service
{
    public class RabbitMQService : IMessageQueueService
    {
        RabbitMQMessagingConnectionFactory connectionFactory;

        public RabbitMQService(RabbitMQMessagingConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task SendAsync<T>(T data, string queueName)
        {
            using var connection = this.connectionFactory.Get();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
 
            var serializedObject = JsonConvert.SerializeObject(data);
            var body = Encoding.UTF8.GetBytes(serializedObject);
 
            channel.BasicPublish(exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: body);
        }
    }
}

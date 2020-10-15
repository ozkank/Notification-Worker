using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NotificationIntegration.Domain.Object;
using NotificationIntegration.Domain.Object.Options;
using NotificationIntegration.Domain.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoticationIntegration.Infrastructure.Service
{
    public class NotificationConsumerService : BackgroundService
    {
        RabbitMQMessagingConnectionFactory connectionFactory;
        private IModel _channel;
        private IConnection _connection;
        private readonly NotificationOptions options;
        private readonly INoSqlRepository<NotificationDto> mongoRepository;

        public NotificationConsumerService(RabbitMQMessagingConnectionFactory connectionFactory, IOptions<NotificationOptions> options, INoSqlRepository<NotificationDto> mongoRepository)
        {
            this.mongoRepository = mongoRepository;
            this.options = options.Value;
            this.connectionFactory = connectionFactory;
            InitializeRabbitMqListener();
        }

        private void InitializeRabbitMqListener()
        {
            _connection = this.connectionFactory.Get();
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            _channel= _connection.CreateModel();
            _channel.QueueDeclare(queue: this.options.Queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var message = JsonConvert.DeserializeObject<string>(content);

                this.mongoRepository.InsertOneAsync(new NotificationDto() { Message = message });

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume(this.options.Queue, false, consumer);

            return Task.CompletedTask;
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
        }
    }
}

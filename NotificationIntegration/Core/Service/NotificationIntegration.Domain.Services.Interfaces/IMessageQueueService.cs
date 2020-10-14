using System.Threading.Tasks;

namespace NotificationIntegration.Domain.Services.Interfaces
{
    public interface IMessageQueueService
    {
        Task SendAsync<T>(T data, string queueName);
    }
}

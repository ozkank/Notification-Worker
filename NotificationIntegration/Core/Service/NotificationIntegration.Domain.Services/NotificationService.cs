using Microsoft.Extensions.Options;
using NotificationIntegration.Domain.Object.Options;
using NotificationIntegration.Domain.Services.Interfaces;
using System.Threading.Tasks;

namespace NotificationIntegration.Domain.Services
{
    public class NotificationService : INotificationService
    {
        private IMessageQueueService messageQueueService;
        private readonly NotificationOptions options;
        public NotificationService(IMessageQueueService messageQueueService, IOptions<NotificationOptions> options)
        {
            this.messageQueueService = messageQueueService;
            this.options = options.Value;
        }

        public async Task<bool> SendMessage(string message)
        {
            await this.messageQueueService.SendAsync(message, this.options.Queue);
            return true;
        }
    }
}

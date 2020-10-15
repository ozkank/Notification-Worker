using Microsoft.Extensions.Options;
using NotificationIntegration.Domain.Object;
using NotificationIntegration.Domain.Object.Options;
using NotificationIntegration.Domain.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationIntegration.Domain.Services
{
    public class NotificationService : INotificationService
    {
        private IMessageQueueService messageQueueService;
        private readonly INoSqlRepository<NotificationDto> mongoRepository;
        private readonly NotificationOptions options;
        public NotificationService(IMessageQueueService messageQueueService, IOptions<NotificationOptions> options, INoSqlRepository<NotificationDto> mongoRepository)
        {
            this.messageQueueService = messageQueueService;
            this.mongoRepository = mongoRepository;
            this.options = options.Value;
        }

        public async Task<bool> SendMessage(string message)
        {
            await this.messageQueueService.SendAsync(message, this.options.Queue);
            return true;
        }


        public List<NotificationDto> GetMessages()
        {
            return this.mongoRepository.AsQueryable().ToList<NotificationDto>();
        }
    }
}

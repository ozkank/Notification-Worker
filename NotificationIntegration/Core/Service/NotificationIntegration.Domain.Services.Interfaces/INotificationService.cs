using System.Collections.Generic;
using System.Threading.Tasks;
using NotificationIntegration.Domain.Object;

namespace NotificationIntegration.Domain.Services.Interfaces
{
    public interface INotificationService
    {
        Task<bool> SendMessage(string message);
        List<NotificationDto> GetMessages();
    }
}

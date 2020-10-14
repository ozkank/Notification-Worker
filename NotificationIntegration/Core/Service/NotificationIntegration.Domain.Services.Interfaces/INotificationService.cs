using System.Threading.Tasks;

namespace NotificationIntegration.Domain.Services.Interfaces
{
    public interface INotificationService
    {
        Task<bool> SendMessage(string message);
    }
}

using Vigilante.Models;

namespace Vigilante.Services.Interfaces
{
    public interface IVGNotificationService
    {
        public Task AddNotificationAsync(Notification notification);

        public Task<List<Notification>> GetReceivedNotificationsAsync(string userId);

        public Task<List<Notification>> GetSentNotificationsAsync(string userId);

        public Task SendEmailNotificationsByRoleAsync(Notification notification, int companyId, string role);

        public Task SendMembersEmailNotificationsAsync(Notification notification, List<VGUser> members);

        public Task<bool> SendEmailNotificationAsync(Notification notification, string emailSubject);
    }
}

using Vigilante.Data;
using Vigilante.Models;
using Vigilante.Services.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace Vigilante.Services
{
    public class VGNotificationService : IVGNotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IVGRolesService _rolesService;

        public VGNotificationService(ApplicationDbContext context,
                                    IVGRolesService rolesService,
                                     IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
            _rolesService = rolesService;
            _emailSender = emailSender;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            try
            {
                await _context.AddAsync(notification);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Notification>> GetReceivedNotificationsAsync(string userId)
        {
            try
            {
                List<Notification> notifications = await _context.Notifications
                                                                 .Include(n => n.Recipient)
                                                                 .Include(n => n.Sender)
                                                                 .Include(n => n.Ticket)
                                                                    .ThenInclude(t => t.Project)
                                                                 .Where(n => n.RecipientId == userId).ToListAsync();
                return notifications;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async  Task<List<Notification>> GetSentNotificationsAsync(string userId)
        {
            try
            {
                List<Notification> notifications = await _context.Notifications
                                                                 .Include(n => n.Recipient)
                                                                 .Include(n => n.Sender)
                                                                 .Include(n => n.Ticket)
                                                                 .ThenInclude(t => t.Project)
                                                                 .Where(n => n.SenderId == userId).ToListAsync();
                return notifications;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> SendEmailNotificationAsync(Notification notification, string emailSubject)
        {
            try
            {
                VGUser vgUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == notification.RecipientId);

                string vgUserEmail = vgUser!.Email;
                string message = notification.Message!;

                //Send Email
                try
                {
                    await _emailSender.SendEmailAsync(vgUserEmail, emailSubject, message);
                    return true;
                }
                catch (Exception)
                {

                    throw;
                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task SendEmailNotificationsByRoleAsync(Notification notification, int companyId, string role)
        {
            try
            {
                List<VGUser> members = await _rolesService.GetUsersInRoleAsync(role, companyId);

                foreach (VGUser vgUser in members)
                {
                    notification.RecipientId = vgUser.Id;
                    await SendEmailNotificationAsync(notification, notification.Title);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task SendMembersEmailNotificationsAsync(Notification notification, List<VGUser> members)
        {
            try
            {
                foreach (VGUser vgUser in members)
                {
                    notification.RecipientId = vgUser.Id;
                    await SendEmailNotificationAsync(notification, notification.Title);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

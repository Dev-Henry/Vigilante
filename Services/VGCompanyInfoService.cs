using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using Vigilante.Data;
using Vigilante.Models;
using Vigilante.Services.Interfaces;

namespace Vigilante.Services
{
    public class VGCompanyInfoService : IVGCompanyInfoService
    {
        private readonly ApplicationDbContext _context; 

        //setting up the constructor - dependency injection
        public VGCompanyInfoService(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<List<VGUser>> GetAllMembersAsync(int companyId)
        {
            List<VGUser> result = new();
            result = await _context.Users.Where(u => u.CompanyId == companyId).ToListAsync();
            return result;
        }

        public async Task<List<Project>> GetAllProjectsAsync(int companyId)
        {
            List<Project> result = new();

            result = await _context.Projects.Where(p => p.CompanyId == companyId)
                                             .Include(p => p.Members)
                                             .Include(p => p.Tickets)
                                                .ThenInclude(t => t.Comments)
                                             .Include(p => p.Tickets)
                                                .ThenInclude(t => t.Attachments)
                                             .Include(p => p.Tickets)
                                                .ThenInclude(t => t.History)
                                             .Include(p => p.Tickets)
                                                .ThenInclude(t => t.Notifications)
                                            .Include(p => p.Tickets)
                                                .ThenInclude(t => t.DeveloperUser)
                                            .Include(p => p.Tickets)
                                                .ThenInclude(t => t.OwnerUser)
                                             .Include(p => p.Tickets)
                                                .ThenInclude(t => t.TicketStatus)
                                             .Include(p => p.Tickets)
                                                 .ThenInclude(t => t.TicketPriority)
                                            .Include(p=>p.Tickets)
                                                 .ThenInclude(t => t.TicketType)
                                            .Include(p=>p.ProjectPriority)
                                            .ToListAsync();
            return result;
        }

        public async Task<List<Ticket>> GetAllTicketAsync(int companyId)
        {
            List<Ticket> result = new();

            //get all tickets using the navigation properties that allows you to get tickets
            List<Project> projects = new();

            projects = await GetAllProjectsAsync(companyId);

            result = projects.SelectMany(p => p.Tickets).ToList();

            return result;
        }

        public async Task<Company> GetCompanyInfoByIdAsync(int? companyId)
        {
            Company result = new();

            if (companyId != null)
            {
                result = await _context.Companies
                                    .Include(c => c.Members)
                                    .Include(c => c.Projects)
                                    .Include(c => c.Invites)
                                    .FirstOrDefaultAsync(c => c.Id == companyId);
            }
            return result; 
        }
    }
}

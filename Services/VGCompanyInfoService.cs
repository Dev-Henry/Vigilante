using Microsoft.EntityFrameworkCore;
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
        }

        public async Task<List<Ticket>> GetAllTicketAsync(int companyId)
        {
        }

        public async Task<Company> GetCompanyInfoByIdAsync(int? companyId)
        {
        }
    }
}

using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Vigilante.Data;
using Vigilante.Models;
using Vigilante.Services.Interfaces;

namespace Vigilante.Services
{
    public class VGLookUpService : IVGLookUpService
    {
        private readonly ApplicationDbContext _context;
        public VGLookUpService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProjectPriority>> GetProjectPrioritiesAsync()
        {
            try
            {
                return await _context.ProjectPriorities.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<TicketPriority>> GetTicketPrioritiesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<TicketStatus>> GetTicketStatusesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<TicketType>> GetTicketTypeAsync()
        {
            throw new NotImplementedException();
        }
    }
}

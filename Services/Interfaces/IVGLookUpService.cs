using Vigilante.Models;

namespace Vigilante.Services.Interfaces
{
    public interface IVGLookUpService
    {
        public Task<List<TicketPriority>> GetTicketPrioritiesAsync();

        public Task<List<TicketStatus>> GetTicketStatusesAsync();

        public Task<List<TicketType>> GetTicketTypeAsync();

        public Task<List<ProjectPriority>> GetProjectPrioritiesAsync();
    }
}

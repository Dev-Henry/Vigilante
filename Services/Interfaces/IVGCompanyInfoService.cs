using Vigilante.Models;

namespace Vigilante.Services.Interfaces
{
    public interface IVGCompanyInfoService
    {
        public Task<Company> GetCompanyInfoByIdAsync (int? companyId);

        public Task<List<VGUser>> GetAllMembersAsync(int companyId);

        public Task<List<Project>> GetAllProjectsAsync(int companyId);

        public Task<List<Ticket>> GetAllTicketAsync(int companyId);

    }
}

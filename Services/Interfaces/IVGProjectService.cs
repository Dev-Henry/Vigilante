using Vigilante.Models;

namespace Vigilante.Services.Interfaces
{
    public interface IVGProjectService
    {
        public Task AddNewProjectAsync(Project project);

        public Task<bool> AddProjectManagerAsync(string userId, int projectId);

        public Task<bool> AddUserToProjectAsync(string userId, int projectId);

        public Task ArchiveProjectAsync(Project project);

        public Task<List<Project>> GetAllProjectsByCompany(int companyId);

        public Task<List<Project>> GetAllProjectsByPriority(int companyId, string priorityName);

        public Task<List<VGUser>> GetAllProjectMembersExceptPMAsync(int projectId);

        public Task<List<Project>> GetArchivedProjectsByCompany(int companyId);

        public Task<List<VGUser>> GetDevelopersOnProjectAsync(int projectId);

        public Task<VGUser> GetProjectManagerAsync(int projectId);

        public Task<List<VGUser>> GetProjectMembersByRoleAsync(int projectId, string roleName); 

        public Task<Project> GetProjectByIdAsync(int projectId, int companyId);

        public Task<List<VGUser>> GetSubmittersOnProjectAsync(int projectId);

        public Task<List<VGUser>> GetUsersNotOnProjectAsync (int projectId, int companyId);

        public Task<List<Project>> GetUserProjectsAsync(string userId);

        public Task<bool> IsUserOnProjectAsync(string userId, int projectId);

        public Task<int> LookUpProjectPriorityId(string priorityName);

        public Task RemoveProjectManagerAsync(int projectId);

        public Task RemoveUsersFromProjectByRoleAsync(string role, int projectId);

        public Task RemoveUserFromProjectAsync(string userId, int projectId);

        public Task RestoreProjectAsync(Project project);


        public Task UpdateProjectAsync(Project project);

    }
}

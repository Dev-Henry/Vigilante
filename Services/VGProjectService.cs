using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Vigilante.Data;
using Vigilante.Models;
using Vigilante.Models.ENUMs;
using Vigilante.Services.Interfaces;

namespace Vigilante.Services
{
    public class VGProjectService : IVGProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly IVGRolesService _rolesService;

        //dependency injection

        public VGProjectService(ApplicationDbContext context, IVGRolesService rolesService)
        {
            _context = context;
            _rolesService = rolesService;
        }

        // CRUD operations - Create 
        public async Task AddNewProjectAsync(Project project)
        {
            _context.Add(project);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AddProjectManagerAsync(string userId, int projectId)
        {
            //VGUser currentPM = await GetProjectManagerAsync(projectId);

            ////remove the current PM if necessary 
            //if (currentPM != null)
            //{
            //    try
            //    {
            //        await RemoveProjectManagerAsync(projectId);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Error removing current PM. - Error :{ex.Message}");
            //        return false;
            //    }
            //}

            ////add the new PM
            //try
            //{
            //    await AddProjectManagerAsync(userId, projectId);
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error adding new PM. - Error :{ex.Message}");
            //    return false;
            //}

            try
    {
        // Remove current PM if any
        VGUser currentPM = await GetProjectManagerAsync(projectId);
        if (currentPM != null)
        {
            await RemoveProjectManagerAsync(projectId);
        }

        // Add the new PM
        Project project = await _context.Projects
                                        .Include(p => p.Members)
                                        .FirstOrDefaultAsync(p => p.Id == projectId);

        VGUser newPM = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (project == null || newPM == null)
        {
            return false;
        }

        // Add the PM to the Members if not already
        if (!project.Members.Any(m => m.Id == userId))
        {
            project.Members.Add(newPM);
        }

        // Save to a separate table/assignment logic if needed
        // (e.g., Role-based assignment if you track PMs separately)

        // Save changes
        await _context.SaveChangesAsync();
        return true;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error assigning Project Manager: {ex.Message}");
        return false;
    }
        }

        public async Task<bool> AddUserToProjectAsync(string userId, int projectId)
        {
            VGUser user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                Project project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
                if (!await IsUserOnProjectAsync(userId, projectId))
                {
                    try
                    {
                        project.Members.Add(user);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // CRUD operations - Delete 
        public async Task ArchiveProjectAsync(Project project)
        {
            try
            {
                //soft delete 
                project.Archived = true;
                await UpdateProjectAsync(project);

                //Archive the tickets for the project
                foreach (Ticket ticket in project.Tickets)
                {
                    ticket.ArchivedByProject = true;
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<List<VGUser>> GetAllProjectMembersExceptPMAsync(int projectId)
        {
            List<VGUser> developers = await GetProjectMembersByRoleAsync(projectId, Roles.SeniorManager.ToString());
            List<VGUser> submitters = await GetProjectMembersByRoleAsync(projectId, Roles.Collaborators.ToString());
            List<VGUser> admins = await GetProjectMembersByRoleAsync(projectId, Roles.Admin.ToString());

            List<VGUser> teamMembers = developers.Concat(submitters).Concat(admins).ToList();
            return teamMembers;
        }

        public async Task<List<Project>> GetAllProjectsByCompanyAsync(int companyId)
        {
            List<Project> projects = new();

            projects = await _context.Projects.Where(p => p.CompanyId == companyId && p.Archived == false)
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
                                           .Include(p => p.Tickets)
                                                .ThenInclude(t => t.TicketType)
                                           .Include(p => p.ProjectPriority)
                                           .ToListAsync();

            return projects;
        }

        public async Task<List<Project>> GetAllProjectsByPriority(int companyId, string priorityName)
        {
            List<Project> projects = await GetAllProjectsByCompanyAsync(companyId);
            int priorityId = await LookUpProjectPriorityId(priorityName);

            return projects.Where(p => p.ProjectPriorityId == priorityId).ToList();
        }

        public async Task<List<Project>> GetArchivedProjectsByCompany(int companyId)
        {
            List<Project> projects = await GetAllProjectsByCompanyAsync(companyId);

            return projects.Where(p => p.Archived == true).ToList();
        }


        //GetProjectMembersByRoleAsync makes GetSubmittersOnProjectAsync and GetDevelopersOnProjectAsync redundant
        public async Task<List<VGUser>> GetDevelopersOnProjectAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        // CRUD operations - Read
        public async Task<Project> GetProjectByIdAsync(int projectId, int companyId)
        {
            Project project = await _context.Projects
                                            .Include(p => p.Tickets)
                                            .Include(p => p.Members)
                                            .Include(p => p.ProjectPriority)
                                            .FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == companyId);

            return project;
        }

        public async Task<VGUser> GetProjectManagerAsync(int projectId)
        {
            Project project = await _context.Projects
                                            .Include(p => p.Members)
                                            .FirstOrDefaultAsync(p => p.Id == projectId);

            foreach (VGUser member in project?.Members)
            {
                if (await _rolesService.IsUserInRoleAsync(member, Roles.ProjectManager.ToString()))
                {
                    return member;
                }
            }

            return null;

        }

        public async Task<List<VGUser>> GetProjectMembersByRoleAsync(int projectId, string role)
        {
            Project project = await _context.Projects
                                            .Include(p => p.Members)
                                            .FirstOrDefaultAsync(p => p.Id == projectId);

            List<VGUser> members = new();

            foreach (var user in project.Members)
            {
                if (await _rolesService.IsUserInRoleAsync(user, role))
                {
                    members.Add(user);
                }
            }
            return members;
        }


        //GetProjectMembersByRoleAsync makes GetSubmittersOnProjectAsync and GetDevelopersOnProjectAsync redundant
        public async Task<List<VGUser>> GetSubmittersOnProjectAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Project>> GetUserProjectsAsync(string userId)
        {
            try
            {
                List<Project> userProjects = (await _context.Users
                                                           .Include(u => u.Projects)
                                                                .ThenInclude(p => p.Company)
                                                           .Include(u => u.Projects)
                                                                .ThenInclude(p => p.Members)
                                                            .Include(u => u.Projects)
                                                                .ThenInclude(p => p.Tickets)
                                                            .Include(u => u.Projects)
                                                                .ThenInclude(t => t.Tickets)
                                                                    .ThenInclude(t => t.DeveloperUser)
                                                            .Include(u => u.Projects)
                                                                .ThenInclude(t => t.Tickets)
                                                                    .ThenInclude(t => t.OwnerUser)
                                                            .Include(u => u.Projects)
                                                                .ThenInclude(t => t.Tickets)
                                                                    .ThenInclude(t => t.TicketPriority)
                                                            .Include(u => u.Projects)
                                                                .ThenInclude(t => t.Tickets)
                                                                    .ThenInclude(t => t.TicketStatus)
                                                            .Include(u => u.Projects)
                                                                .ThenInclude(t => t.Tickets)
                                                                    .ThenInclude(t => t.TicketType)
                                                            .FirstOrDefaultAsync(u => u.Id == userId)).Projects.ToList();

                return userProjects;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"**** ERROR **** - Error Getting User Project List. ---> {ex.Message}");
                throw;
            }
        }


        public async Task<List<VGUser>> GetUsersNotOnProjectAsync(int projectId, int companyId)
        {   //asynchronous call
            List<VGUser> users = await _context.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToListAsync();

            return users.Where(u => u.CompanyId == companyId).ToList();
        }

        public async Task<bool> IsUserOnProjectAsync(string userId, int projectId)
        {
            Project project = await _context.Projects
                                            .Include(p => p.Members)
                                            .FirstOrDefaultAsync(p => p.Id == projectId);

            bool result = false;

            if (project != null)
            {
                result = project.Members.Any(m => m.Id == userId);
            }

            return result;
        }

        public async Task<int> LookUpProjectPriorityId(string priorityName)
        {
            int proirityId = (await _context.ProjectPriorities.FirstOrDefaultAsync(p => p.Name == priorityName)).Id;
            return proirityId;
        }

        public async Task RemoveProjectManagerAsync(int projectId)
        {
            Project project = await _context.Projects
                                            .Include(p => p.Members)
                                            .FirstOrDefaultAsync(p => p.Id == projectId);
            try
            {
                foreach (VGUser member in project?.Members)
                {
                    if (await _rolesService.IsUserInRoleAsync(member, Roles.ProjectManager.ToString()))
                    {
                        await RemoveUserFromProjectAsync(member.Id, projectId);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task RemoveUserFromProjectAsync(string userId, int projectId)
        {
            try
            {
                VGUser user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                Project project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

                try
                {
                    if (await IsUserOnProjectAsync(userId, projectId))
                    {
                        project.Members.Remove(user);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"**** ERROR ***** - Error Removing User from the Project. ---> {ex.Message}");
            }
        }

        public async Task RemoveUsersFromProjectByRoleAsync(string role, int projectId)
        {
            try
            {
                List<VGUser> members = await GetProjectMembersByRoleAsync(projectId, role);
                Project project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

                foreach (VGUser vgUser in members)
                {
                    try
                    {
                        project.Members.Remove(vgUser);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"**** ERROR **** - Error Removing Users from Project. ---> {ex.Message}");
                throw;
            }
        }

        public async Task RestoreProjectAsync(Project project)
        {
            try
            {
                //soft delete 
                project.Archived = false;
                await UpdateProjectAsync(project);

                //Archive the tickets for the project
                foreach (Ticket ticket in project.Tickets)
                {
                    ticket.ArchivedByProject = false;
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        // CRUD operations - Update 
        public async Task UpdateProjectAsync(Project project)
        {
            try
            {
                _context.Update(project);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
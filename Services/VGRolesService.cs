using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vigilante.Data;
using Vigilante.Models;
using Vigilante.Services.Interfaces;

namespace Vigilante.Services
{
    public class VGRolesService : IVGRolesService
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<VGUser> _userManager;


        //dependy injection
        public VGRolesService(ApplicationDbContext context,
                                RoleManager<IdentityRole> roleManager,
                                UserManager<VGUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<List<IdentityRole>> GetRolesAsync()
        {
            try
            {
                List<IdentityRole> result = new();

                result = await _context.Roles.ToListAsync();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> AddUserToRoleAsync(VGUser user, string roleName)
        {
            bool result = (await _userManager.AddToRoleAsync(user, roleName)).Succeeded;
            return result;
        }

        public async Task<string> GetRoleNameByIdAsync(string roleId)
        {
            IdentityRole role = _context.Roles.Find(roleId);
            string result = await _roleManager.GetRoleNameAsync(role);
            return result;
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(VGUser user)
        {   
            IEnumerable<string> result = await _userManager.GetRolesAsync(user);
            return result;
        }

        public async Task<List<VGUser>> GetUsersInRoleAsync(string roleName, int companyId)
        {
            List<VGUser> users = (await _userManager.GetUsersInRoleAsync(roleName)).ToList();
            List<VGUser> result = users.Where(u=> u.CompanyId == companyId).ToList();
            return result;
        }

        public async Task<List<VGUser>> GetUsersNotInRoleAsync(string roleName, int companyId)
        {
            List<string> userIds = (await _userManager.GetUsersInRoleAsync(roleName)).Select(u => u.Id).ToList();
            List<VGUser> roleUsers = _context.Users.Where(u => !userIds.Contains(u.Id)).ToList(); 
            
            List <VGUser> result = roleUsers.Where(u => u.CompanyId == companyId).ToList();
            return result;
        }

        public async Task<bool> IsUserInRoleAsync(VGUser user, string roleName)
        {
            bool result = await _userManager.IsInRoleAsync(user, roleName);
            return result;
        }

        public async Task<bool> RemoveUserFromRolesAsync(VGUser user, string roleName)
        {
            bool result = (await _userManager.RemoveFromRoleAsync(user, roleName)).Succeeded;
            return result;
        }

        public async Task<bool> RemoveUserFromRolesAsync(VGUser user, IEnumerable<string> roles)
        {
            bool result = (await _userManager.RemoveFromRolesAsync(user, roles)).Succeeded;    
            return result;
        }
    }
}

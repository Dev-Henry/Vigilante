using Microsoft.AspNetCore.Identity;
using Vigilante.Models;

namespace Vigilante.Services.Interfaces
{
    public interface IVGRolesService
    {
        //everything in an interface is public whether typed out or not

        //add threading to keep the application running smoothly 
        public Task<bool> IsUserInRoleAsync(VGUser user, string roleName);

        public Task<List<IdentityRole>> GetRolesAsync();


        public Task<IEnumerable<string>> GetUserRolesAsync (VGUser user);

        public Task<bool> AddUserToRoleAsync (VGUser user, string roleName);

        public Task<bool> RemoveUserFromRolesAsync(VGUser user, string roleName);

        public Task<bool> RemoveUserFromRolesAsync(VGUser user, IEnumerable <string> roles);

        //intro of multi tenancy by passing around company ID
        public Task<List<VGUser>> GetUsersInRoleAsync(string roleName, int companyId);

        public Task<List<VGUser>> GetUsersNotInRoleAsync(string roleName, int companyId);

        //this will help maintain the numbering of any List 
        public Task<string> GetRoleNameByIdAsync(string roleId);

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Crypto.Modes.Gcm;
using Vigilante.Extensions;
using Vigilante.Models;
using Vigilante.Models.ViewModels;
using Vigilante.Services.Interfaces;

namespace Vigilante.Controllers
{
    [Authorize]
    public class UserRolesController : Controller
    {
        private readonly IVGRolesService _rolesService;
        private readonly IVGCompanyInfoService _companyInfoService;

        public UserRolesController (IVGRolesService rolesService,
                                    IVGCompanyInfoService companyInfoService)
        {
            _rolesService = rolesService;
            _companyInfoService = companyInfoService;
        }

        [HttpGet]
        public async Task <IActionResult> ManageUserRoles()
        {
            //add an instance of the ViewModel as a list (model)
            List<ManageUserRolesViewModel> model = new();

            //Get companyId
            int companyId = User.Identity.GetCompanyId().Value;

            //Get all company users 
            List<VGUser> users = await _companyInfoService.GetAllMembersAsync (companyId);

            //loop over the users to populate the ViewModel
            // - instanstiate ViewModel
            // - user _rolesService 
            // - Create multi-selects
            foreach (VGUser user in users)
            {
                ManageUserRolesViewModel viewModel = new();
                viewModel.VGUser = user;

                //intermediate step
                IEnumerable<string> selected = await _rolesService.GetUserRolesAsync(user);
                viewModel.Roles = new MultiSelectList(await _rolesService.GetRolesAsync(), "Name", "Name", selected);

                model.Add(viewModel);
            }

            //Return the model to the View  
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUserRoles(ManageUserRolesViewModel member)
        {
            //Get the CompanyId
            int companyId = User.Identity.GetCompanyId().Value;

            //Instantiate the VGUser
            VGUser vgUser = (await _companyInfoService.GetAllMembersAsync(companyId)).FirstOrDefault(u => u.Id == member.VGUser.Id);

            //Get the Roles for the User 
            IEnumerable<string> roles = await _rolesService.GetUserRolesAsync(vgUser);

            //Grab the selected role 
            string userRole = member.SelectedRoles.FirstOrDefault();

            if (!string.IsNullOrEmpty(userRole))
            {
                //remove user from their roles
                if(await _rolesService.RemoveUserFromRolesAsync(vgUser, roles))
                {
                    //add user to the new role 
                    await _rolesService.AddUserToRoleAsync(vgUser, userRole);
                }

            }

            //Navigate back to the view
            return RedirectToAction(nameof(ManageUserRoles));
        }   
    }
}

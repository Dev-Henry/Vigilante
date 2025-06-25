using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Vigilante.Extensions;
using Vigilante.Models;
using Vigilante.Models.ViewModels;
using Vigilante.Services.Interfaces;

namespace Vigilante.Controllers
{
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
    }
}

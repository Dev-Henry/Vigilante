using Microsoft.AspNetCore.Mvc.Rendering;

namespace Vigilante.Models.ViewModels
{
    public class ManageUserRolesViewModel
    {
        //view models are a means for transporting data around 
        public VGUser VGUser { get; set; }

        public MultiSelectList Roles { get; set; }

        public List<string> SelectedRoles { get; set; }
    }
}

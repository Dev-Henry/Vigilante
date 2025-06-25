using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Vigilante.Models;

namespace Vigilante.Services.Factories
{
    public class VGUserClaimsPrincipleFactory : UserClaimsPrincipalFactory<VGUser, IdentityRole>
    {
        public VGUserClaimsPrincipleFactory(UserManager<VGUser> userManager,
                                            RoleManager<IdentityRole> roleManager,
                                            IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
        {

        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(VGUser user)
        {
            ClaimsIdentity identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("CompanyId", user.CompanyId.ToString()));

            return identity;

            //// Note: The above code assumes that VGUser has a property called CompanyId.
        }
    }
}

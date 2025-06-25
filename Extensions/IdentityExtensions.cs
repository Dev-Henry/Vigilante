using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;
using System.Security.Principal;

namespace Vigilante.Extensions
{
    public static class IdentityExtensions
    {
        public static int? GetCompanyId(this IIdentity identity)
        {
            Claim claim = ((ClaimsIdentity)identity).FindFirst("CompandId");

            //Ternancy operator (if/else)
            return (claim !=null) ? int.Parse(claim.Value) :null;
        }
    }
}

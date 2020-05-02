using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Revoow.Areas.Identity
{
    public class RevoowUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<RevoowUser, IdentityRole>
    {
        public RevoowUserClaimsPrincipalFactory(
            UserManager<RevoowUser> userManager,
            RoleManager<IdentityRole> roleManager, 
            IOptions<IdentityOptions> options) : 
            base(userManager, roleManager, options)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(RevoowUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            identity.AddClaim(new Claim("AccountType", user.AccountType.ToString()));
            identity.AddClaim(new Claim("MaxVideos", user.MaxVideos.ToString()));

            return identity;

        }
    }
}

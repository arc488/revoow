﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}

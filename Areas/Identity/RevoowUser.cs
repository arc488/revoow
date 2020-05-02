using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revoow.Areas.Identity
{
    public class RevoowUser : IdentityUser
    {
        public int MaxVideos { get; set; }
        public AccountType AccounntType { get; set; }
    }

    public enum AccountType
    {
        Free,
        Small,
        Professional
    };
}

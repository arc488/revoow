using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revoow.Areas.Identity
{
    public class RevoowUser : IdentityUser
    {
        private int maxVideos;
        public int MaxVideos
        {
            get
            {
                switch (AccountType)
                {
                    case AccountType.Professional:
                        this.maxVideos = 10000;
                        break;
                    case AccountType.Small:
                        this.maxVideos = 10;
                        break;
                    case AccountType.Free:
                        this.maxVideos = 5;
                        break;
                    default:
                        this.maxVideos = 5;
                        break;
                }
                return this.maxVideos;
            }
            set
            {
                maxVideos = 5;
            }
        }
        public AccountType AccountType { get; set; }
    }

    public enum AccountType
    {
        Free,
        Small,
        Professional
    };
}

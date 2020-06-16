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
                switch (SubscriptionType)
                {
                    case SubscriptionType.Professional:
                        this.maxVideos = 10000;
                        break;
                    case SubscriptionType.Small:
                        this.maxVideos = 10;
                        break;
                    case SubscriptionType.Starter:
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
        public SubscriptionType SubscriptionType { get; set; }
        public string SubscriptionId { get; set; }
        public string CustomerId { get; set; }

        public bool IsSubscribed()
        {
            return !String.IsNullOrEmpty(SubscriptionId);
        }

    }


}

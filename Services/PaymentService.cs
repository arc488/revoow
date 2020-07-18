using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Revoow.Areas.Identity;
using Revoow.Options;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Revoow.Services
{
    public class PaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly string _smallBusinessPlanId;
        private readonly string _professionalPlanId;

        public PaymentService(IConfiguration configuration,
                              IOptions<StripeOptions> stripeOptions)
        {
            _configuration = configuration;
            _smallBusinessPlanId = stripeOptions.Value.Plans.SmallBusinessPlanId;
            _professionalPlanId = stripeOptions.Value.Plans.ProfessionalPlanId;
        }

        public Session CreateSession(SubscriptionType type, string hostHeader, RevoowUser user)
        {
            // Set your secret key. Remember to switch to your live secret key in production!
            // See your keys here: https://dashboard.stripe.com/account/apikeys

            var options = new SessionCreateOptions
            {

                PaymentMethodTypes = new List<string> {
                    "card",
                },
                SubscriptionData = new SessionSubscriptionDataOptions
                {
                    Metadata = new Dictionary<string, string>
                    {
                        { "UserEmail", user.Email },
                        { "PendingDowngrade", false.ToString() }

                    }, 
                    Items = new List<SessionSubscriptionDataItemOptions> {
                        new SessionSubscriptionDataItemOptions {
                            Plan = this.GetPlanIdFromEnum(type),
                            Quantity = 1,
                            

                        },
                            
                    },
                },
                SuccessUrl = "https://" + hostHeader + "/company/create",
                CancelUrl = "https://" + hostHeader + "/",


            };

            var service = new SessionService();
            Session session = service.Create(options);

            return session;
        }

        public Subscription UpgradeSubscription(RevoowUser user, SubscriptionType type)
        {
            var service = new SubscriptionService();
            Subscription subscription = service.Get(user.SubscriptionId);

            var items = new List<SubscriptionItemOptions> {
                new SubscriptionItemOptions {
                    Id = subscription.Items.Data[0].Id,
                    Plan = this.GetPlanIdFromEnum(type),
                    Metadata = new Dictionary<string, string>
                    {
                        {"UserEmail", user.Email },
                        {"PendingDowngrade", false.ToString() }

                    }
                },
            };

            var options = new SubscriptionUpdateOptions
            {
                CancelAtPeriodEnd = false,
                ProrationBehavior = "always_invoice",
                Items = items
            };

            subscription = service.Update(user.SubscriptionId, options);

            return subscription;

        }

        public Subscription DowngradeSubscription(RevoowUser user, SubscriptionType type)
        {
            var service = new SubscriptionService();
            Subscription subscription = service.Get(user.SubscriptionId);

            var items = new List<SubscriptionItemOptions> {
                new SubscriptionItemOptions {
                    Id = subscription.Items.Data[0].Id,
                    Plan = this.GetPlanIdFromEnum(type),

                },
            };

            var options = new SubscriptionUpdateOptions
            {
                CancelAtPeriodEnd = false,
                ProrationBehavior = "none",
                Metadata = new Dictionary<string, string>
                {
                    { "UserEmail", user.Email },
                    { "PendingDowngrade", true.ToString() }
                },
                Items = items
            };

            subscription = service.Update(user.SubscriptionId, options);
         
            return subscription;
        }

        public Subscription ChangeDowngradePendingStatus(Subscription subscription, bool isPending)
        {
            var service = new SubscriptionService();

            var options = new SubscriptionUpdateOptions
            {
                Metadata = new Dictionary<string, string>
                {
                    { "PendingDowngrade", isPending.ToString() }
                },
            };

            return service.Update(subscription.Id, options);

        }



        public Session RetrieveSession(string sessionId)
        {
            var service = new SessionService();
            var session = service.Get(sessionId);
            return session;
        }

        public Subscription PauseSubscription(string subscriptionId)
        {
            var options = new SubscriptionUpdateOptions
            {
                PauseCollection = new SubscriptionPauseCollectionOptions
                {
                    Behavior = "mark_uncollectible",                 
                },
            };
            var service = new SubscriptionService();
            var subscription = service.Update(subscriptionId, options);
            return subscription;
        }

        public Subscription UnpauseSubscription(string subscriptionId)
        {
            var options = new SubscriptionUpdateOptions();
            options.AddExtraParam("pause_collection", "");
            var service = new SubscriptionService();
            var subscription = service.Update(subscriptionId, options);
            return subscription;
        }
        
        public Subscription CancelSubscription(string subscriptionId)
        {
            var service = new SubscriptionService();
            var options = new SubscriptionUpdateOptions
            {
                CancelAtPeriodEnd = true,
                Prorate = false,
            };
           
            var subscription = service.Update(subscriptionId, options);
            return subscription;

        }

        public Subscription RetrieveSubscription(string subscriptionId)
        {
            var service = new SubscriptionService();
            var subscription = service.Get(subscriptionId);
            return subscription;
        }

        public SubscriptionType GetEnumFromPlanId(string plan)
        {
            SubscriptionType type;

            if (plan == _smallBusinessPlanId)
            {
                type = SubscriptionType.Small;
            }
            else if (plan == _professionalPlanId)
            {
                type = SubscriptionType.Professional;
            }
            else
            {
                type = SubscriptionType.Starter;
            }

            return type;
        }

        public string GetPlanIdFromEnum(SubscriptionType type)
        {
            string planId;

            if (type == SubscriptionType.Small)
            {
                planId = _smallBusinessPlanId;
            }
            else if (type == SubscriptionType.Professional)
            {
                planId = _professionalPlanId;
            }
            else
            {
                planId = "";
            }

            return planId;
        }

        public bool HasPendingDowngrade(Subscription subscription)
        {
            return (subscription.Metadata["PendingDowngrade"].Contains("True"));
        }

    }

}

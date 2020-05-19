using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Revoow.Areas.Identity;
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
        private readonly IConfiguration configuration;
        private readonly UserManager<RevoowUser> userManager;
        private readonly string smallPlanId;
        private readonly string professionalPlanId;

        public PaymentService(IConfiguration configuration,
                              UserManager<RevoowUser> userManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            StripeConfiguration.ApiKey = this.configuration["Stripe:SecretKey"];
            this.smallPlanId = this.configuration["Stripe:Plans:Small"];
            this.professionalPlanId = this.configuration["Stripe:Plans:Professional"];
        }

        public Session CreateSession(SubscriptionType type, string hostHeader)
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
                    Items = new List<SessionSubscriptionDataItemOptions> {
                        new SessionSubscriptionDataItemOptions {                      
                            Plan = GetPlanIdFromSubscriptionType(type),                         
                        },
                    },
                },
                SuccessUrl = "https://" + hostHeader + "/Payment/Success/{CHECKOUT_SESSION_ID}",
                CancelUrl = "https://" + hostHeader + "/cancel",

            };

            var service = new SessionService();
            Session session = service.Create(options);

            return session;
        }

        public Subscription ChangePlan(string userId, SubscriptionType type)
        {
            var user = this.userManager.FindByIdAsync(userId).Result;

            bool isUpgrade = (int)type > (int)user.SubscriptionType;

            return new Subscription();
        }
 
        public Subscription UpdateSubscription(string userId, SubscriptionType type)
        {
            var user = this.userManager.FindByIdAsync(userId).Result;
            var currentType = user.SubscriptionType;
            var subscriptionId = user.SubscriptionId;

            bool isUpgrade = (int)type > (int)currentType;

            var service = new SubscriptionService();
            Subscription subscription = service.Get(subscriptionId);

            var items = new List<SubscriptionItemOptions> {
                new SubscriptionItemOptions {               
                    Id = subscription.Items.Data[0].Id,
                    Plan = GetPlanIdFromSubscriptionType(type),
                },
            };

            string behavior; 
            if (isUpgrade)
            {
                behavior = "always_invoice";
            }
            else
            {
                behavior = "none";
            };

            var options = new SubscriptionUpdateOptions
            {
                CancelAtPeriodEnd = false,
                ProrationBehavior = behavior,           
                Items = items
            };

            subscription = service.Update(subscriptionId, options);

            return subscription;
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
            var cancelOptions = new SubscriptionCancelOptions
            {
                
                InvoiceNow = false,
                Prorate = false,
            };
            
            var subscription = service.Cancel(subscriptionId, cancelOptions);
            return subscription;

        }

        public Subscription RetrieveSubscription(string subscriptionId)
        {
            var service = new SubscriptionService();
            var subscription = service.Get(subscriptionId);
            return subscription;
        }

        public string GetPlanIdFromSubscriptionType(SubscriptionType type)
        {
            string planId = "";

            switch ((int)type)
            {
                case 1:
                    planId = this.configuration["Stripe:Plans:Small"];
                    break;
                case 2:
                    planId = this.configuration["Stripe:Plans:Professional"];
                    break;
            }

            return planId;

        }

    }

}

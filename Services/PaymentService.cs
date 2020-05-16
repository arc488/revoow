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

        public PaymentService(IConfiguration configuration,
                              UserManager<RevoowUser> userManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            StripeConfiguration.ApiKey = this.configuration["Stripe:SecretKey"];
        }

        public Session CreateSession(string planId, HttpRequest request, int quantity)
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
                            Plan = planId,
                            Quantity = quantity
                        },
                    },
                },
                SuccessUrl = "https://" + request.Host + "/Payment/Success/{CHECKOUT_SESSION_ID}",
                CancelUrl = "https://" + request.Host + "/cancel",

            };

            var service = new SessionService();
            Session session = service.Create(options);

            return session;
        }

        public Subscription UpdateSubscription(string userId, Areas.Identity.AccountType type)
        {
            Debug.WriteLine("Inside UpdateSubscription");
            var user = this.userManager.FindByIdAsync(userId).Result;
            var currentType = user.AccountType;
            var subscriptionId = user.SubscriptionId;

            bool isUpgrade = (int)type > (int)currentType;

            var service = new SubscriptionService();
            Subscription subscription = service.Get(subscriptionId);

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
                Quantity = (int)type,
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
    }

}

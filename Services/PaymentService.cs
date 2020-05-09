using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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

        public PaymentService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Session CreateSession(string planId, HttpRequest request)
        {
            // Set your secret key. Remember to switch to your live secret key in production!
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.ApiKey = this.configuration["Stripe:SecretKey"];
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

        public Session RetrieveSession(string sessionId)
        {
            StripeConfiguration.ApiKey = this.configuration["Stripe:SecretKey"];

            var service = new SessionService();
            var session = service.Get(sessionId);
            return session;
            
        }
    }

}

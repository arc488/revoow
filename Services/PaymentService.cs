using Microsoft.AspNetCore.Http;
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
        public Session Pay(string planId, HttpRequest request)
        {
            // Set your secret key. Remember to switch to your live secret key in production!
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.ApiKey = "sk_test_hakGpiFBNLIxXwfge40vA36V006DjNpIUY";
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
            Debug.WriteLine(options.SuccessUrl);
            Debug.WriteLine(options.CancelUrl);

            var service = new SessionService();
            Session session = service.Create(options);

            return session;
        }
    }

}

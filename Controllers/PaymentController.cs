using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Revoow.Areas.Identity;
using Revoow.Services;
using Stripe.Checkout;

namespace Revoow.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly PaymentService paymentService;
        private readonly UserManager<RevoowUser> userManager;
        private readonly IConfiguration configuration;

        public PaymentController(PaymentService paymentService,
                                 UserManager<RevoowUser> userManager,
                                 IConfiguration configuration
                                 )
        {
            this.paymentService = paymentService;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public IActionResult Pay(SubscriptionType id)
        {
            var type = id;
            var hostHeader = Request.Host.ToString();

            if (type != SubscriptionType.Starter)
            {
                Session session = paymentService.CreateSession(type, hostHeader);
                ViewBag.sessionId = session.Id;
                return View();
            };
            return RedirectToAction("Success");
        }

 
        public async Task<IActionResult> Success(string id)
        {
            var user = userManager.GetUserAsync(HttpContext.User).Result;
            if (!string.IsNullOrEmpty(id))
            {
                var session = this.paymentService.RetrieveSession(id);

                user.CustomerId = session.CustomerId;
                user.SubscriptionId = session.SubscriptionId;

                var subscription = this.paymentService.RetrieveSubscription(user.SubscriptionId);

                SubscriptionType type = (SubscriptionType)subscription.Quantity;

                user.SubscriptionType = type;
            }
            else
            {
                user.SubscriptionType = SubscriptionType.Starter;
            };

            await userManager.UpdateAsync(user);
            
            return View();
        }

    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Revoow.Areas.Identity;
using Revoow.Services;
using Stripe.Checkout;

namespace Revoow.Controllers
{
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

        public IActionResult Pay(AccountType id)
        {
            string planId = GetPlanIdFromAccountType(id);
            Session session = paymentService.CreateSession(planId, Request);
            ViewBag.sessionId = session.Id;
            return View();
        }

        public async Task<IActionResult> Success(string id)
        {
            var user = userManager.GetUserAsync(HttpContext.User).Result;
            var session = this.paymentService.RetrieveSession(id);

            var plan = session.DisplayItems.First().Plan;
            AccountType type = GetAccountTypeFromString(plan.Id);

            Debug.WriteLine("User email: " + user.Email);

            user.AccountType = type;
            if (plan.Active)
            {
                Debug.WriteLine("Before update async");
                await userManager.UpdateAsync(user);
            }


            return View();
        }


        #region Helpers
        public AccountType GetAccountTypeFromString(string acconuntTypeString)
        {
            AccountType type;
            if (acconuntTypeString == this.configuration["Stripe:PlanId:Small"])
            {
                type = AccountType.Small;
            }
            else if (acconuntTypeString == this.configuration["Stripe:PlanId:Professional"])
            {
                type = AccountType.Professional;
            }
            else if (acconuntTypeString == this.configuration["Stripe:PlanId:Free"])
            {
                type = AccountType.Free;
            }
            else
            {
                type = AccountType.Free;
            }
            return type;
        }

        public string GetPlanIdFromAccountType(AccountType type)
        {
            string planId;
            switch (type)
            {
                case AccountType.Small:
                    planId = this.configuration["Stripe:PlanId:Small"];
                    break;
                case AccountType.Professional:
                    planId = this.configuration["Stripe:PlanId:Professional"];
                    break;
                case AccountType.Free:
                    planId = this.configuration["Stripe:PlanId:Free"];
                    break;
                default:
                    planId = "free";
                    break;
            }
            return planId;
        } 
        #endregion

    }
}
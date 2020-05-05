using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Revoow.Areas.Identity;
using Revoow.Services;
using Stripe.Checkout;

namespace Revoow.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PaymentService paymentService;

        public PaymentController(PaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        public IActionResult Pay(AccountType id)
        {
            string planId;
            Debug.WriteLine(id);

            switch (id)
            {
                case AccountType.Small:
                    planId = "plan_HDTxpLwX95ov78";
                    break;
                case AccountType.Professional:
                    planId = "plan_HDTxIMxXtq1sC8";
                    break;
                default:
                    planId = "free";
                    break;
            }
            Debug.WriteLine(planId);


            if ((planId == "free")) {
                Debug.WriteLine("Plan id is free");
            };

            Session session = paymentService.Pay(planId, Request);
            ViewBag.sessionId = session.Id;
            return View();
        }

        public IActionResult Success(string id)
        {
            Debug.WriteLine("Payment successful " + id);
            return View();
        }
    }
}
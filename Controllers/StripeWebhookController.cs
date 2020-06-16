using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Stripe;
using Microsoft.Extensions.Options;
using Revoow.Options;
using Revoow.Services;
using Microsoft.AspNetCore.Identity;
using Revoow.Areas.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace Revoow.Controllers
{
    [Route("webhooks")]
    [ApiController]
    [AllowAnonymous]
    public class StripeWebhookController : ControllerBase
    {
        private readonly UserManager<RevoowUser> _userManager;
        private readonly StripeEventHandler _handler;
        private readonly PaymentService _service;
        private readonly string _webhookSecret;

        public StripeWebhookController(IOptions<StripeOptions> options,
                                       UserManager<RevoowUser> userManager,
                                       StripeEventHandler handler,
                                       PaymentService service)
        {
            _userManager = userManager;
            _handler = handler;
            _service = service;
            _webhookSecret = options.Value.WebhookSigningKey;
        }


        [HttpPost]
        public async Task<IActionResult> Post()
        {
            string json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], _webhookSecret);
                var stripeObject = DeserializeStripeObjectFromJson(json);
                switch (stripeEvent.Type)
                {
                    case Events.CustomerSubscriptionUpdated:
                        await _handler.HandleCustomerSubscriptionUpdated((Subscription)stripeObject);
                        break;
                    case Events.InvoicePaymentSucceeded:
                        await _handler.HandleInvoicePaymentSucceeded((Invoice)stripeObject);
                        break;
                    case Events.InvoiceCreated:
                        await  _handler.HandleInvoiceCreated((Invoice)stripeObject);
                        break;
                    case Events.CustomerSubscriptionDeleted:
                        await _handler.HandleSubscriptionDeleted((Subscription)stripeObject);
                        break;
                    default:
                        Debug.WriteLine("Default case");
                        break;
                }
                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }


        public object DeserializeStripeObjectFromJson(string json)
        {
            JObject eventJson = JObject.Parse(json);
            var objectJson = eventJson["data"]["object"].ToString();
            var objectType = eventJson["data"]["object"]["object"].ToString();
            if (objectType == "invoice")
            {
                return JsonConvert.DeserializeObject<Invoice>(objectJson);
            }
            else if (objectType == "subscription")
            {
                return JsonConvert.DeserializeObject<Subscription>(objectJson);
            }

            return new object();
        }

    }
}

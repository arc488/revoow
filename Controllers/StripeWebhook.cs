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

namespace Revoow.Controllers
{
    [Route("webhooks")]
    [ApiController]
    [AllowAnonymous]
    public class StripeWebhook : ControllerBase
    {
        private readonly PaymentService _service;
        private readonly UserManager<RevoowUser> _userManager;
        private readonly string _webhookSecret;

        public StripeWebhook(IOptions<StripeOptions> options,
                             PaymentService service,
                             UserManager<RevoowUser> userManager)
        {
            _service = service;
            _userManager = userManager;
            _webhookSecret = options.Value.WebhookSigningKey;
        }


        [HttpPost]
        public async Task<IActionResult> Post()
        {
            string json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            JObject eventJson = JObject.Parse(json);
            var objectJson = eventJson["data"]["object"].ToString();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], _webhookSecret);
                //Debug.WriteLine("Stripe event is: " + stripeEvent.Type);

                switch (stripeEvent.Type)
                {
                    case Events.CustomerSubscriptionUpdated:
                        Subscription subscription = JsonConvert.DeserializeObject<Subscription>(objectJson);

                        Debug.WriteLine("Extracted");
                        Debug.WriteLine(subscription);
                        Debug.WriteLine(subscription.Id);

                        break;
                    case Events.CustomerSourceExpiring:
                        //send reminder email to update payment method
                        break;
                    case Events.ChargeFailed:
                        //do something
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
    }
}

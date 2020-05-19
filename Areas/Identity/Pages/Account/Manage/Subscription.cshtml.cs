using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Revoow.Areas.Identity;
using Revoow.Services;

namespace Revoow.Areas.Identity.Pages.Account.Manage
{
    public partial class SubscriptionModel : PageModel
    {
        private readonly UserManager<RevoowUser> _userManager;
        private readonly SignInManager<RevoowUser> _signInManager;
        private readonly PaymentService _paymentService;

        public SubscriptionModel(
            UserManager<RevoowUser> userManager,
            SignInManager<RevoowUser> signInManager,
            PaymentService paymentService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _paymentService = paymentService;
        }

        public string Username { get; set; }

        public string Expiration { get; set; }

        public SubscriptionType SubscriptionType { get; set; }

        public bool IsCanceled { get; set; }

        public int CurrentType { get; set; }


        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Password")]
            public string Password { get; set; }

            public SubscriptionType SubscriptionType { get; set; }

        }

        private async Task LoadAsync(RevoowUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            Username = userName;
            SubscriptionType = user.SubscriptionType;
            CurrentType = (int)user.SubscriptionType;
            IsCanceled = _paymentService.RetrieveSubscription(user.SubscriptionId).CanceledAt != null;

            if (user.SubscriptionType != SubscriptionType.Starter)
            {
                var subscription = _paymentService.RetrieveSubscription(user.SubscriptionId);
                Expiration = subscription.CurrentPeriodEnd.Value.ToShortDateString();
            }
            else
            {
                Expiration = DateTime.MaxValue.ToShortDateString();
            }

            Input = new InputModel
            {
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var redirectUrl = "~/Identity/Account/Manage/Subscription";

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (user.SubscriptionType != Input.SubscriptionType)
            {
                var newSubscriptionType = Input.SubscriptionType;
                var isCanceled = _paymentService.RetrieveSubscription(user.SubscriptionId).CanceledAt != null;

                if (newSubscriptionType != SubscriptionType.Starter && newSubscriptionType != user.SubscriptionType && isCanceled == false)
                {
                    var subscription = _paymentService.UpdateSubscription(user.Id, newSubscriptionType);
                    var newType = (SubscriptionType)subscription.Quantity;
                    user.SubscriptionType = newType;

                }
                else if (newSubscriptionType == SubscriptionType.Starter)
                {
                    var subscription = _paymentService.CancelSubscription(user.SubscriptionId);
                }
                else if (user.SubscriptionType == SubscriptionType.Starter && String.IsNullOrEmpty(user.SubscriptionId))
                {
                    redirectUrl = "~/Payment/Pay/" + newSubscriptionType;
                }
                else if (isCanceled && String.IsNullOrEmpty(user.SubscriptionId) == false)
                {
                    redirectUrl = "~/Payment/Pay/" + newSubscriptionType;
                }
                else
                {
                    await LoadAsync(user);
                    return Page();
                }
            }

            await _userManager.UpdateAsync(user);
            //StatusMessage = "Your profile has been updated";
            return Redirect(redirectUrl);
        }
    }
}

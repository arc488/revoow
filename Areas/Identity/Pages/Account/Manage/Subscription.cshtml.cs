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

        [BindProperty]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public class InputModel
        {

            [Range(minimum: 1, maximum: 2)]
            public SubscriptionType SubscriptionType { get; set; }

        }

        private async Task LoadAsync(RevoowUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            Username = userName;
            SubscriptionType = user.SubscriptionType;
            CurrentType = (int)user.SubscriptionType;

            if (user.IsSubscribed())
            {
                IsCanceled = _paymentService.RetrieveSubscription(user.SubscriptionId).CancelAtPeriodEnd == true;
                if (user.SubscriptionType != SubscriptionType.Starter)
                {
                    var subscription = _paymentService.RetrieveSubscription(user.SubscriptionId);
                    Expiration = subscription.CurrentPeriodEnd.Value.ToShortDateString();
                }
                else
                {
                    Expiration = DateTime.MaxValue.ToShortDateString();
                }

            } 
            else
            {
                IsCanceled = false;
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

        public async Task<IActionResult> OnPostCancelSubscriptionAsync()
        {
            Debug.WriteLine("CancelSubscription called");
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, Password, false);
            if (result == Microsoft.AspNetCore.Identity.SignInResult.Success)
            {
                _paymentService.CancelSubscription(user.SubscriptionId);
                StatusMessage = "Your subscription has been canceled";
            }
            else
            {
                StatusMessage = "Provided password is incorrect, please try again.";
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



            //If the new subscription isn't the same as old 
            if (user.SubscriptionType != Input.SubscriptionType)
            {
                var newSubscriptionType = Input.SubscriptionType;
                var isUpgrade = (int)newSubscriptionType > (int)user.SubscriptionType;

                if (user.IsSubscribed())
                {
                    if (isUpgrade)
                    {
                        _paymentService.UpgradeSubscription(user, newSubscriptionType);
                    }
                    else if (!isUpgrade)
                    {
                        _paymentService.DowngradeSubscription(user, newSubscriptionType);
                    }
                }


                if  (user.IsSubscribed() == false)
                {
                    redirectUrl = "~/Payment/Pay/" + newSubscriptionType;
                }

            }

            await _userManager.UpdateAsync(user);
            //StatusMessage = "Your profile has been updated";
            return Redirect(redirectUrl);
        }
    }
}

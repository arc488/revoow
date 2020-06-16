using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Revoow.Areas.Identity;
using Revoow.Services;
using Stripe;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Revoow.Controllers
{
    public class StripeEventHandler
    {
        private readonly PaymentService _service;
        private readonly UserManager<RevoowUser> _userManager;

        public StripeEventHandler(PaymentService service,
                                  UserManager<RevoowUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        public async Task<bool> HandleCustomerSubscriptionUpdated(Subscription subscription)
        {
            var user = await GetUserFromSubscritpion(subscription);
            var type = _service.GetEnumFromPlanId(subscription.Plan.Id);

            if ((int)user.SubscriptionType > (int)type)
            {
                Debug.WriteLine("This was a downgrade");
            }

            return await UpdateUserConcurrent(user, "prio");

        }

        public async Task<bool> HandleInvoiceCreated(Invoice invoice)
        {
            var subscription = _service.RetrieveSubscription(invoice.SubscriptionId);
            var user = await GetUserFromSubscritpion(subscription);
            bool hasPendingDowngrade = _service.HasPendingDowngrade(subscription);
            Debug.WriteLine("HasPendingDowngrade: " + hasPendingDowngrade.ToString());
            if (hasPendingDowngrade)
            {
                user.SubscriptionType = _service.GetEnumFromPlanId(subscription.Plan.Id);
                _service.ChangeDowngradePendingStatus(subscription, false);
            }

            return await UpdateUserConcurrent(user, String.Empty);
        }

        public async Task<bool> HandleInvoicePaymentSucceeded(Invoice invoice)
        {
            Debug.WriteLine("HandleInvoicePaymentSucceeded");
            var subscription = _service.RetrieveSubscription(invoice.SubscriptionId);
            var user = await GetUserFromSubscritpion(subscription);

            //var user = _userManager.Users.Where(u => u.Email == subscription.Metadata["userEmail"]).FirstOrDefault();

            var type = _service.GetEnumFromPlanId(subscription.Plan.Id);

            Debug.WriteLine("IsSubscribed: " + user.IsSubscribed());

            if (user.IsSubscribed() == false)
            {
                Debug.WriteLine("User is not subscribed");
                user.SubscriptionType = type;
                user.SubscriptionId = invoice.SubscriptionId;
                user.CustomerId = invoice.CustomerId;
            }
            else
            {
                //Upgrade
                Debug.WriteLine("User is subscribed");
                if ((int)user.SubscriptionType < (int)type)
                {
                    user.SubscriptionType = type;
                    Debug.WriteLine("This was an upgrade");
                }
            }

            Debug.WriteLine("UserSub type: " + user.SubscriptionType);
            return await UpdateUserConcurrent(user, String.Empty);
        }
        
        public Task<RevoowUser> GetUserFromSubscritpion(Subscription subscription)
        {
            var user = _userManager.FindByEmailAsync(subscription.Metadata["UserEmail"]);
            return user;
        }

        public async Task<bool> HandleSubscriptionDeleted(Subscription subscription)
        {
            var user = await _userManager.FindByEmailAsync(subscription.Metadata["UserEmail"]);
            user.SubscriptionType = SubscriptionType.Starter;
            user.SubscriptionId = null;
            user.CustomerId = null;
            var result = await UpdateUserConcurrent(user, String.Empty);
            return result;
        }

        public async Task<bool> UpdateUserConcurrent(RevoowUser user, string priority)
        {
            Debug.WriteLine("UpdateUserConcurrent");
            var saved = false;

            while (!saved)
            {
                try
                {
                    await _userManager.UpdateAsync(user);
                    saved = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Debug.WriteLine("Caught exception");
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is RevoowUser)
                        {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();

                            foreach (var property in proposedValues.Properties)
                            {
                                var proposedValue = proposedValues[property];
                                var databaseValue = databaseValues[property];
                                Debug.WriteLine("ProposedValue: " + proposedValue);
                                Debug.WriteLine("DatabaseValue: " + databaseValue);
                                Debug.WriteLine("PropertyName: " + property.Name);
                                Debug.WriteLine("SubscriptionType string: " + user.SubscriptionType.GetType().Name);
                                if (priority == "prio" && property.Name == user.SubscriptionType.GetType().Name)
                                {
                                    user.SubscriptionType = (SubscriptionType)proposedValue;
                                }
                                // TODO: decide which value should be written to database
                                // proposedValues[property] = <value to be saved>;
                            }

                            // Refresh original values to bypass next concurrency check
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                        else
                        {
                            throw new NotSupportedException(
                                "Don't know how to handle concurrency conflicts for "
                                + entry.Metadata.Name);
                        }
                    }
                }
            }
            return saved;
        }



    }
}



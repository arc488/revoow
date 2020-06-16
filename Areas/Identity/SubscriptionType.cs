using Microsoft.Extensions.Options;
using Revoow.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revoow.Areas.Identity
{
    public class SubscriptionTypeHelpers
    {
        private readonly IOptions<StripeOptions> _stripeOptions;
        private readonly string _smallBusinessPlanId;
        private readonly string _professionalPlanId;

        public SubscriptionTypeHelpers(IOptions<StripeOptions> stripeOptions)
        {
            _stripeOptions = stripeOptions;
            _smallBusinessPlanId = _stripeOptions.Value.Plans.SmallBusinessPlanId;
            _professionalPlanId = _stripeOptions.Value.Plans.ProfessionalPlanId;
        }
        

    }

    public enum SubscriptionType
    {
        Starter,
        Small,
        Professional
    };
}

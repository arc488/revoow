using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revoow.Options
{
    public class StripeOptions
    {
        public string WebhookSigningKey { get; set; }
        public StripePlans Plans { get; set; }

    }

    public class StripePlans
    {
        public string SmallBusinessPlanId { get; set; }
        public string ProfessionalPlanId { get; set; }
    }
}

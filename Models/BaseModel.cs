using Revoow.Areas.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revoow.Models
{
    public class BaseModel
    {
        public RevoowUser CreatedBy { get; set; }
        public RevoowUser ModifiedBy { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
    }
}

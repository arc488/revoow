using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revoow.Models
{
    public class BaseModel
    {
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
    }
}

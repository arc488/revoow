using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revoow.ViewModels.Revoow
{
    public class UploadViewModel
    {
        public int PageId { get; set; }
        public byte[] Logo { get; set; }
        public string CompanyName { get; set; }
        public string RevoowURL { get; set; }
    }
}

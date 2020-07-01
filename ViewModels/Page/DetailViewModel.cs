using Revoow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revoow.ViewModels.Revoow
{
    public class DetailViewModel
    {
        public byte[] Logo { get; set; }
        public string CompanyName { get; set; }
        public string PageURL { get; set; }
        public Testimonial[] Testimonials { get; set; }
        public int CompanyRating { get; set; }
        public bool IsVideoLimitReached { get; set; }
    }
}

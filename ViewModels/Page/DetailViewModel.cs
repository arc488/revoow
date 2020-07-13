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
        public string WebsiteUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
    }
}

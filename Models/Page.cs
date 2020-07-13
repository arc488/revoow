using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Revoow.Models
{
    public class Page : BaseModel
    {
        [Key]
        public int PageId { get; set; }
        public byte[] Logo { get; set; }
        public string CompanyName { get; set; }
        public string PageURL { get; set; }
        public string WebsiteUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public List<Testimonial> Testimonials { get; set; }
        public int CompanyRating
        {
            get
            {
                if (Testimonials.Count > 0) return (int)Math.Round(Testimonials.Select(t => t.Rating).Average());
                else return 5;
            }
        }
    }
}

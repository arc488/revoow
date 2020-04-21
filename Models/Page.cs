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

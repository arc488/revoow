using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revoow.Models
{
    public class Testimonial : BaseModel
    {
        public int TestimonialID { get; set; }
        public int PageId { get; set; }
        public int Rating { get; set; }
        public string VideoName { get; set; }
        public byte[] VideoThumbnail { get; set; }
        public string ReviewerName { get; set; }
    }
}

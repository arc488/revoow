﻿using System;
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
    }
}

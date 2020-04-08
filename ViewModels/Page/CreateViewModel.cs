using Revoow.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Revoow.ViewModels.Revoow
{
    public class CreateViewModel
    {
        public byte[] Logo { get; set; }
        [Required, StringLength(30, MinimumLength = 2, ErrorMessage = "Address must be between 2 and 30 characters long")]
        [Display(Name = "Company Name")]
        [CompanyNameIsUnique]
        public string CompanyName { get; set; }
        [Required, StringLength(30, MinimumLength = 2, ErrorMessage = "Address must be between 2 and 30 characters long")]
        [Display(Name = "Landing Page Address")]
        public string PageURL { get; set; }
        public List<String> URLsInUse { get; set; }
        public List<String> NamesInUse { get; set; }
    }
}
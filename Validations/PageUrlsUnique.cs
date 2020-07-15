using Revoow.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Revoow.Validations
{
    public class PageUrlIsUnique : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("This field cannot be empty.");
            }
            var pageURL = value.ToString();
            var appDbContext = (AppDbContext)validationContext
                         .GetService(typeof(AppDbContext));
            var result = appDbContext.Pages.Count(p => p.PageURL == pageURL);
            if (result > 0)
            {
                return new ValidationResult("This address is taken.");
            };
            return ValidationResult.Success;

        }
    }

}

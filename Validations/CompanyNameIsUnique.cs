using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Revoow.Data;
using Revoow.Data.Repositories;
using System.Diagnostics;

namespace Revoow.Validations
{
    public class CompanyNameIsUnique: ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Company name cannot be empty");
            }

            var companyName = value.ToString();
            Debug.WriteLine("Company name is: " + companyName);
            var appDbContext = (AppDbContext)validationContext
                         .GetService(typeof(AppDbContext));
            var result = appDbContext.Pages.Count(p => p.CompanyName == companyName);
            Debug.WriteLine("Result was: " + result);
            if (result > 0)
            {
                Debug.WriteLine("Validation failed");
                return new ValidationResult("This company name is taken.");
            };
            Debug.WriteLine("Validation successful");
            return ValidationResult.Success;

        }
    }
}

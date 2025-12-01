using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Airdnd.Models.Validation
{
    public class BathroomAttribute : ValidationAttribute, IClientModelValidator
    {
        // Server-side validation logic
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is double bathrooms)
            {
                // allow whole numbers or .5 increments
                var frac = bathrooms % 1;
                if (Math.Abs(frac) < 0.000001 || Math.Abs(frac - 0.5) < 0.000001)
                {
                    return ValidationResult.Success!;
                }
            }

            return new ValidationResult(ErrorMessage ?? "Number of bathrooms must be a whole or half number (e.g., 1, 1.5, 2).");
        }

        // Adding unobtrusive attributes for client-side validation
        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null) return;

            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-bathroom", ErrorMessage ?? "Number of bathrooms must be a whole or half number (e.g., 1, 1.5, 2).");
        }
    }
}
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Airdnd.Models.Validation
{
    public class BuiltYearAttribute : ValidationAttribute, IClientModelValidator
    {
        private int _maxYearsAgo;

        // Setting a default of 150 years and allowing override
        public BuiltYearAttribute(int maxYearsAgo = 150)
        {
            _maxYearsAgo = maxYearsAgo;
        }

        // Server-side validation logic
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is int year)
            {
                int currentYear = DateTime.Now.Year;
                int minYear = currentYear - _maxYearsAgo;

                if (year >= minYear && year <= currentYear)
                {
                    return ValidationResult.Success!;
                }
                else
                {
                    return new ValidationResult(GetErrorMessage(minYear, currentYear));
                }
            }

            return new ValidationResult("Please enter a valid year.");
        }

        // Adding unobtrusive attributes for client-side unobtrusive validation
        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null) return;

            int current = DateTime.Now.Year;
            int min = current - _maxYearsAgo;

            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-builtyear", GetErrorMessage(min, current));
            context.Attributes.Add("data-val-builtyear-min", min.ToString());
            context.Attributes.Add("data-val-builtyear-max", current.ToString());
        }

        private string GetErrorMessage(int min, int max)
        {
            return ErrorMessage ?? $"Year must be between {min} and {max}.";
        }
    }
}
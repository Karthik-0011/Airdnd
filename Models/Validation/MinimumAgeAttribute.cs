using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Airdnd.Models.Validation
{
    public class MinimumAgeAttribute : ValidationAttribute, IClientModelValidator
    {
        private int _minYears;
        
        public MinimumAgeAttribute(int years)
        {
            _minYears = years;
        }

        // Server-side validation
        protected override ValidationResult IsValid(object? value, ValidationContext ctx)
        {
            if (value is DateTime)
            {
                DateTime dateToCheck = (DateTime)value;
                dateToCheck = dateToCheck.AddYears(_minYears);
                if (dateToCheck <= DateTime.Today)
                {
                    return ValidationResult.Success!;
                }
            }
            return new ValidationResult(GetMsg(ctx.DisplayName ?? "Date"));
        }

        // Client-side validation
        public void AddValidation(ClientModelValidationContext ctx)
        {
            if (ctx == null) return;

            ctx.Attributes.Add("data-val", "true");
            ctx.Attributes.Add("data-val-minimumage-years", _minYears.ToString());
            ctx.Attributes.Add("data-val-minimumage", GetMsg(ctx.ModelMetadata.DisplayName ?? ctx.ModelMetadata.Name ?? "Date"));
        }

        private string GetMsg(string name)
        {
            return ErrorMessage ?? $"{name} must be at least {_minYears} years ago.";
        }
    }
}
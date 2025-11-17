using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Airdnd.Models
{
    public class User : IValidatableObject
    {
        public int UserId { get; set; } 

        [Required(ErrorMessage = "Please enter a name.")]
        [StringLength(50, ErrorMessage = "Name must be 50 characters or less.")]
        public string Name { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Remote("CheckEmail", "Validation", "Admin")]
        public string? Email { get; set; }
        
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string? PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Please enter a date of birth.")]
        [MinimumAge(18, ErrorMessage = "User must be at least 18 years old.")]
        public DateTime? DOB { get; set; }
        public string? SSN { get; set; }
        
        [Required(ErrorMessage = "Please select a user type.")]
        public string UserType { get; set; } = string.Empty; 

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(PhoneNumber) && string.IsNullOrEmpty(Email))
            {
                yield return new ValidationResult(
                    "Either Phone Number or an Email must be provided.",
                    new[] { nameof(PhoneNumber), nameof(Email) }
                );
            }
        }
    }
}
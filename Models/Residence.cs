using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Airdnd.Models
{
    public class Residence
    {
        public int ResidenceId { get; set; }
        
        [Required(ErrorMessage = "Please enter a name.")]
        [StringLength(50, ErrorMessage = "Name must be 50 characters or less.")]
        [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "Name must be alphanumeric.")]
        public string Name { get; set; } = string.Empty;
        
        public string ResidencePicture { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Please enter guest accommodation number.")]
        [Range(1, 100, ErrorMessage = "Accommodation must be a valid number.")]
        public int GuestNumber { get; set; }
        
        [Required(ErrorMessage = "Please enter number of bedrooms.")]
        [Range(1, 50, ErrorMessage = "Bedrooms must be a valid number.")]
        public int BedroomNumber { get; set; }
        
        [Required(ErrorMessage = "Please enter number of bathrooms.")]
        [Range(1, 20, ErrorMessage = "Bathrooms must be between 1 and 20.")]
        [Bathroom]
        public double BathroomNumber { get; set; }

        [Required(ErrorMessage = "Please enter a price.")]
        [Range(1, 20000, ErrorMessage = "Price must be a valid number.")]
        public double PricePerNight { get; set; }

        [Required(ErrorMessage = "Please select a location.")]
        public string LocationId { get; set; } = string.Empty;
        
        public Location? Location { get; set; }

        [Required(ErrorMessage = "Please enter the year built.")]
        [BuiltYear(150, ErrorMessage = "Year must be a past year but no more than 150 years ago.")]
        public int BuiltYear { get; set; } 

        [Required(ErrorMessage = "Please select an owner.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select an owner.")]
        [Remote("CheckOwner", "Validation", "Admin")]
        public int UserId { get; set; } 

        public User? Owner { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Airdnd.Models.DomainModels
{
    public class Location
    {
        [Required(ErrorMessage = "Location ID is required.")]
        [RegularExpression("^[a-z0-9]+$", ErrorMessage = "Location ID must contain only lowercase letters or digits (no spaces).")]
        public string LocationId { get; set; } = string.Empty; 

        [Required(ErrorMessage = "Location name is required.")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Name must contain only letters and spaces.")]
        public string Name { get; set; } = string.Empty;
    }
}
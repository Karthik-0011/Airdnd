using System.ComponentModel.DataAnnotations;

namespace Airdnd.Models
{
    public class Location
    {
        [Required(ErrorMessage = "Location ID is required.")]
        public string LocationId { get; set; } = string.Empty; 

        [Required(ErrorMessage = "Location name is required.")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Name must contain only letters and spaces.")]
        public string Name { get; set; } = string.Empty;
    }
}
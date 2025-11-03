using System.ComponentModel.DataAnnotations;
namespace Airdnd.Models
{
    public class Client
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
    }
}
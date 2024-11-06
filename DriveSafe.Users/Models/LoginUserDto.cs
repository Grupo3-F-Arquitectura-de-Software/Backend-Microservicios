using System.ComponentModel.DataAnnotations;

namespace DriveSafe.Users.Models
{
    public class LoginUserDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        
        [Required]
        public required string Password { get; set; }
    }
}
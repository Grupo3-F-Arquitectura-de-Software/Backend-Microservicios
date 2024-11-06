using System.ComponentModel.DataAnnotations;

namespace DriveSafe.Users.Models
{
    public class RegisterUserDto
    {
        [Required]
        public required string Name { get; set; }
        
        [Required]
        public required string LastName { get; set; }
        
        [Required]
        public DateTime Birthdate { get; set; }
        
        [Required]
        public required string Cellphone { get; set; }
        
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        
        [Required]
        [MinLength(6)]
        public required string Password { get; set; }
        
        [Required]
        public required string Type { get; set; }
    }
}
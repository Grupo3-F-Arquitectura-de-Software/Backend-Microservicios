namespace DriveSafe.Users.Models
{
    public class UpdateUserDto
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? Cellphone { get; set; }
        public string? Email { get; set; }
    }
}
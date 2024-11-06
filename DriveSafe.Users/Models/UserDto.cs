namespace DriveSafe.Users.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public required string Cellphone { get; set; }
        public required string Email { get; set; }
        public required string Type { get; set; }
    }
}
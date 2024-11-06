namespace DriveSafe.Users.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public required string Cellphone { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Type { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
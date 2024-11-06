using DriveSafe.Users.Models;

namespace DriveSafe.Users.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        int? ValidateToken(string token);
    }
}
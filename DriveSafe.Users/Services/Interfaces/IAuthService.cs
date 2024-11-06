using DriveSafe.Users.Models;

namespace DriveSafe.Users.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> RegisterAsync(RegisterUserDto registerDto);
        Task<string> LoginAsync(LoginUserDto loginDto);
    }
}
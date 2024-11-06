using System;
using System.Threading.Tasks;
using AutoMapper;
using DriveSafe.Users.Models;
using DriveSafe.Users.Repositories.Interfaces;
using DriveSafe.Users.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using BC = BCrypt.Net.BCrypt;

namespace DriveSafe.Users.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IMapper mapper, ITokenService tokenService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        public async Task<UserDto> RegisterAsync(RegisterUserDto registerDto)
        {
            if (await _userRepository.GetByEmailAsync(registerDto.Email) != null)
            {
                throw new InvalidOperationException("Email is already in use");
            }

            var user = _mapper.Map<User>(registerDto);
            user.Password = HashPassword(registerDto.Password);

            await _userRepository.CreateAsync(user);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<string> LoginAsync(LoginUserDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null || !VerifyPassword(loginDto.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            return _tokenService.GenerateToken(user);
        }

        private string HashPassword(string password)
        {
            return BC.HashPassword(password, BC.GenerateSalt());
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BC.Verify(password, hashedPassword);
        }
    }
}
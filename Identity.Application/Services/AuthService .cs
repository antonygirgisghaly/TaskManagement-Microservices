using Identity.Application.Comman;
using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using Identity.Domain.Comman;
using Identity.Domain.Entites;
using Identity.Infrastructure.Interfaces;
using Identity.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<Result<AuthResponseDto>> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if(user is null)
                return Result<AuthResponseDto>.Failure("Invalid email or password");
            var isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
                return Result<AuthResponseDto>.Failure("Invalid email or password");
            var Token = _tokenService.GenerateToken(user);
            var response = new AuthResponseDto
            {
                FullName = user.FullName,
                Email = user.Email,
                Token = Token
            };
            return Result<AuthResponseDto>.Success(response);
        }

        public async Task<Result<AuthResponseDto>> RegisterAsync(RegisterRequestDto request)
        {
            var user = await _userRepository.ExistsUserByEmailAsync(request.Email);
            if (user)
                return Result<AuthResponseDto>.Failure("Email already registered");
            var data = new User
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = _passwordHasher.Hash(request.Password)
            };
            await _userRepository.AddUserAsync(data);
            var Token = _tokenService.GenerateToken(data);
            var response = new AuthResponseDto
            {
                FullName = data.FullName,
                Email = data.Email,
                Token = Token
            };
            return Result<AuthResponseDto>.Success(response);
        }
    }
}

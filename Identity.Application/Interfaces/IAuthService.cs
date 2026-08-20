using System;
using System.Collections.Generic;
using System.Text;
using Identity.Application.Comman;
using Identity.Application.DTOs;
namespace Identity.Infrastructure.Settings
{
    public interface IAuthService
    {
        Task<Result<AuthResponseDto>> RegisterAsync(RegisterRequestDto request);
        Task<Result<AuthResponseDto>> LoginAsync(LoginRequestDto request);
    }
}

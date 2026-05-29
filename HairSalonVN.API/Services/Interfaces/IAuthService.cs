using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HairSalonVN.API.Services.DTOs.Auth;
using HairSalonVN.API.Services.DTOs.Common;
using HairSalonVN.Database.Entities;

namespace HairSalonVN.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<TokenDto>> RegisterAsync(RegisterDto dto);
        Task<ApiResponse<TokenDto>> LoginAsync(LoginDto dto);
        Task<ApiResponse<TokenDto>> RefreshTokenAsync(string token);
        Task RevokeTokenAsync(string token);
        Task<User?> GetUserByIdAsync(Guid id);
    }
}

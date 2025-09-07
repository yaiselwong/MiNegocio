using Microsoft.EntityFrameworkCore;
using MiNegocio.Server.Interfaces;
using MiNegocio.Shared.Data;
using MiNegocio.Shared.Data.UoW;
using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Dto.Response;
using MiNegocio.Shared.Models;
using System;

namespace MiNegocio.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;

        public AuthService(IUnitOfWork unitOfWork, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetAll()
                    .Include(u => u.Company)
                    .FirstOrDefaultAsync(u => u.Username == request.Username);

                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Invalid username or password"
                    };
                }

                user.LastLoginAt = DateTime.UtcNow;
                 _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.CommitAsync();

                var token = _jwtService.GenerateToken(user);

                return new AuthResponse
                {
                    Success = true,
                    Token = token,
                    Message = "Login successful",
                    User = user
                };
            }
            catch (Exception ex)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = $"Login failed: {ex.Message}"
                };
            }
        }

        public async Task<AuthResponse> RegisterAsync(User user, string password)
        {
            try
            {
                if (await _unitOfWork.UserRepository.GetAll().AnyAsync(u => u.Username == user.Username))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Username already exists"
                    };
                }

                if (await _unitOfWork.UserRepository.GetAll().AnyAsync(u => u.Email == user.Email))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Email already exists"
                    };
                }

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
                user.CreatedAt = DateTime.UtcNow;

                await _unitOfWork.UserRepository.Add(user);
                await _unitOfWork.CommitAsync();

                var token = _jwtService.GenerateToken(user);

                return new AuthResponse
                {
                    Success = true,
                    Token = token,
                    Message = "Registration successful",
                    User = user
                };
            }
            catch (Exception ex)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = $"Registration failed: {ex.Message}"
                };
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using MiNegocio.Server.Interfaces;
using MiNegocio.Shared.Data.UoW;
using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Models;

namespace MiNegocio.Server.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _unitOfWork.UserRepository.GetAll()
                .Include(u => u.Company)
                .OrderBy(u => u.Username)
                .ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _unitOfWork.UserRepository.GetAll()
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<User>> GetUsersByCompanyAsync(int companyId)
        {
            return await _unitOfWork.UserRepository.GetAll(u => u.CompanyId == companyId)
                .Include(u => u.Company)
                .OrderBy(u => u.Username)
                .ToListAsync();
        }

        public async Task<User> CreateUserAsync(CreateUserRequest request)
        {
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Role = request.Role,
                CompanyId = request.CompanyId,
                CreatedAt = DateTime.UtcNow,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            await _unitOfWork.UserRepository.Add(user);
            await _unitOfWork.CommitAsync();

            return await GetUserByIdAsync(user.Id);
        }

        public async Task<User?> UpdateUserAsync(UpdateUserRequest request)
        {
            var user = await _unitOfWork.UserRepository.FindOneAsync(u => u.Id == request.Id);

            if (user == null) return null;

            user.Username = request.Username;
            user.Email = request.Email;
            user.Role = request.Role;
            user.CompanyId = request.CompanyId;

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.CommitAsync();

            return await GetUserByIdAsync(user.Id);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.FindOneAsync(u => u.Id == id);

            if (user == null) return false;

             _unitOfWork.UserRepository.Delete(user);
            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> ResetPasswordAsync(int id, string newPassword)
        {
            var user = await _unitOfWork.UserRepository.FindOneAsync(u => u.Id == id);

            if (user == null) return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
             _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}

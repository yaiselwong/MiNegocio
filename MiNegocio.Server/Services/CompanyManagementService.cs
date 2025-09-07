using Microsoft.EntityFrameworkCore;
using MiNegocio.Server.Interfaces;
using MiNegocio.Shared.Data.UoW;
using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Models;

namespace MiNegocio.Server.Services
{
    public class CompanyManagementService : ICompanyManagementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Company>> GetAllCompaniesAsync()
        {
            return await _unitOfWork.CompanyRepository.GetAll()
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Company?> GetCompanyByIdAsync(int id)
        {
            return await _unitOfWork.CompanyRepository.FindOneAsync(c => c.Id == id);
        }

        public async Task<Company> CreateCompanyAsync(CreateCompanyRequest request)
        {
            var company = new Company
            {
                Name = request.Name,
                Address = request.Address,
                Phone = request.Phone,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.CompanyRepository.Add(company);
            await _unitOfWork.CommitAsync();

            return company;
        }

        public async Task<Company?> UpdateCompanyAsync(int id, CreateCompanyRequest request)
        {
            var company = await _unitOfWork.CompanyRepository.FindOneAsync(c => c.Id == id);

            if (company == null) return null;

            company.Name = request.Name;
            company.Address = request.Address;
            company.Phone = request.Phone;
            company.Email = request.Email;
            company.UpdatedAt = DateTime.UtcNow;

             _unitOfWork.CompanyRepository.Update(company);
            await _unitOfWork.CommitAsync();

            return company;
        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            var company = await _unitOfWork.CompanyRepository.FindOneAsync(c => c.Id == id);
            if (company == null) return false;

            // Check if there are users associated with this company
            var hasUsers = await _unitOfWork.UserRepository.GetAll().AnyAsync(u => u.CompanyId == id);
            if (hasUsers)
            {
                return false; // Cannot delete company with associated users
            }

             _unitOfWork.CompanyRepository.Delete(company);
            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}

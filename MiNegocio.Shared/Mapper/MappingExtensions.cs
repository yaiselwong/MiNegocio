using Mapster;
using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Dto.Response;
using MiNegocio.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Shared.Mapper
{
    public static class MappingExtensions
    {
        // Mapeo de User a UserDto
        public static UserDto ToDto(this User user)
        {
            return user.Adapt<UserDto>();
        }

        public static List<UserDto> ToDto(this List<User> users)
        {
            return users.Adapt<List<UserDto>>();
        }

        // Mapeo de Company a CompanyDto
        public static CompanyDto ToDto(this Company company)
        {
            return company.Adapt<CompanyDto>();
        }

        public static List<CompanyDto> ToDto(this List<Company> companies)
        {
            return companies.Adapt<List<CompanyDto>>();
        }

        // Mapeo de CreateUserRequest a User
        public static User ToEntity(this CreateUserRequest request)
        {
            return request.Adapt<User>();
        }

        // Mapeo de UpdateUserRequest a User
        public static User ToEntity(this UpdateUserRequest request)
        {
            return request.Adapt<User>();
        }

        // Mapeo de CreateCompanyRequest a Company
        public static Company ToEntity(this CreateCompanyRequest request)
        {
            return request.Adapt<Company>();
        }
    }
}

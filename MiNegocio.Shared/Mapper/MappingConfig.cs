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
    public static class MappingConfig
    {
        public static void RegisterMappings()
        {
            // Mapeo de User a UserDto
            TypeAdapterConfig<User, UserDto>
                .NewConfig()
                .Map(dest => dest.Company, src => src.Company != null ? new CompanyDto
                {
                    Id = src.Company.Id,
                    Name = src.Company.Name,
                    Address = src.Company.Address,
                    Phone = src.Company.Phone,
                    Email = src.Company.Email,
                    CreatedAt = src.Company.CreatedAt,
                    UpdatedAt = src.Company.UpdatedAt
                } : null);

            // Mapeo de Company a CompanyDto
            TypeAdapterConfig<Company, CompanyDto>
                .NewConfig()
                .Map(dest => dest.Users, src => src.Users.Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Role = u.Role,
                    CompanyId = u.CompanyId,
                    CreatedAt = u.CreatedAt,
                    LastLoginAt = u.LastLoginAt
                }).ToList());

            // Mapeo de CreateUserRequest a User
            TypeAdapterConfig<CreateUserRequest, User>
                .NewConfig()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.PasswordHash)
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.LastLoginAt);

            // Mapeo de UpdateUserRequest a User
            TypeAdapterConfig<UpdateUserRequest, User>
                .NewConfig()
                .Ignore(dest => dest.PasswordHash)
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.LastLoginAt);

            // Mapeo de CreateCompanyRequest a Company
            TypeAdapterConfig<CreateCompanyRequest, Company>
                .NewConfig()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.UpdatedAt);

            // Warehouse mappings
            TypeAdapterConfig<Warehouse, WarehouseDto>
                .NewConfig()
                .Map(dest => dest.ProductCount, src => src.Products != null ? src.Products.Count : 0);

            TypeAdapterConfig<CreateWarehouseRequest, Warehouse>
                .NewConfig()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.UpdatedAt)
                .Ignore(dest => dest.IsActive)
                .Ignore(dest => dest.CompanyId);
            // Category mappings
            TypeAdapterConfig<Category, CategoryDto>
                .NewConfig()
                .Map(dest => dest.ProductCount, src => src.Products != null ? src.Products.Count : 0);

            TypeAdapterConfig<CreateCategoryRequest, Category>
                .NewConfig()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.UpdatedAt)
                .Ignore(dest => dest.IsActive)
                .Ignore(dest => dest.CompanyId);

            // UnitOfMeasure mappings
            TypeAdapterConfig<UnitOfMeasure, UnitOfMeasureDto>
                .NewConfig()
                .Map(dest => dest.ProductCount, src => src.Products != null ? src.Products.Count : 0);

            TypeAdapterConfig<CreateUnitOfMeasureRequest, UnitOfMeasure>
                .NewConfig()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.UpdatedAt)
                .Ignore(dest => dest.IsActive)
                .Ignore(dest => dest.CompanyId);
        }
    }
}

using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MiNegocio.Client.Shared.Interfaces;
using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Client.Shared.Services
{
    public class AdminService : IAdminService
    {
        private readonly AuthorizedHttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navigationManager;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AdminService(
           AuthorizedHttpClient httpClient,
           ILocalStorageService localStorage,
           NavigationManager navigationManager,
           AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _navigationManager = navigationManager;
            _authenticationStateProvider = authenticationStateProvider;
        }
       
        public async Task<List<User>> GetAllUsersAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<User>>("api/usermanagement");
            return response ?? new List<User>();
        }

        public async Task<List<User>> GetUsersByCompanyAsync(int companyId)
        {
            var response = await _httpClient.GetFromJsonAsync<List<User>>($"api/usermanagement/company/{companyId}");
            return response ?? new List<User>();
        }

        public async Task<User?> GetUserAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<User>($"api/usermanagement/{id}");
            return response;
        }

        public async Task<User?> CreateUserAsync(CreateUserRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/usermanagement", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<User>();
            }
            return null;
        }

        public async Task<User?> UpdateUserAsync(UpdateUserRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/usermanagement/{request.Id}", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<User>();
            }
            return null;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/usermanagement/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ResetPasswordAsync(int id, string newPassword)
        {
            var request = new ResetPasswordRequest { NewPassword = newPassword };
            var response = await _httpClient.PostAsJsonAsync($"api/usermanagement/{id}/reset-password", request);
            return response.IsSuccessStatusCode;
        }

        // Company Management Methods
        public async Task<List<Company>> GetAllCompaniesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Company>>("api/companymanagement");
            return response ?? new List<Company>();
        }

        public async Task<Company?> GetCompanyAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<Company>($"api/companymanagement/{id}");
            return response;
        }

        public async Task<Company?> CreateCompanyAsync(CreateCompanyRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/companymanagement", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Company>();
            }
            return null;
        }

        public async Task<Company?> UpdateCompanyAsync(int id, CreateCompanyRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/companymanagement/{id}", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Company>();
            }
            return null;
        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/companymanagement/{id}");
            return response.IsSuccessStatusCode;
        }
    }

}

using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
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
    public class AuthorizedHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly NavigationManager _navigationManager;

        public AuthorizedHttpClient(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authStateProvider,
            NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
            _navigationManager = navigationManager;
        }

        private async Task AddAuthorizationHeader()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        private async Task HandleUnauthorizedResponse()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("user");

            if (_authStateProvider is CustomAuthStateProvider authStateProvider)
            {
                var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
                authStateProvider.NotifyStateChanged(new AuthenticationState(anonymous));
            }

            _navigationManager.NavigateTo("/login");
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            try
            {
                await AddAuthorizationHeader();
                var response = await _httpClient.GetAsync(requestUri);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await HandleUnauthorizedResponse();
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T value)
        {
            try
            {
                await AddAuthorizationHeader();
                var response = await _httpClient.PostAsJsonAsync(requestUri, value);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await HandleUnauthorizedResponse();
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en PostAsJsonAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<HttpResponseMessage> PutAsJsonAsync<T>(string requestUri, T value)
        {
            try
            {
                await AddAuthorizationHeader();
                var response = await _httpClient.PutAsJsonAsync(requestUri, value);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await HandleUnauthorizedResponse();
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en PutAsJsonAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            try
            {
                await AddAuthorizationHeader();
                var response = await _httpClient.DeleteAsync(requestUri);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await HandleUnauthorizedResponse();
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en DeleteAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<T?> GetFromJsonAsync<T>(string requestUri)
        {
            try
            {
                await AddAuthorizationHeader();
                var response = await _httpClient.GetAsync(requestUri);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await HandleUnauthorizedResponse();
                    return default;
                }

                return await response.Content.ReadFromJsonAsync<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetFromJsonAsync: {ex.Message}");
                return default;
            }
        }
    }
}

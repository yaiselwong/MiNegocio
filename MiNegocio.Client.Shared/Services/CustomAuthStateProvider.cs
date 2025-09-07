using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Dto.Response;
using MiNegocio.Shared.Models;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;

namespace MiNegocio.Client.Shared.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public CustomAuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");

                if (string.IsNullOrEmpty(token))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAuthenticationStateAsync: {ex.Message}");
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public async Task<bool> LoginAsync(LoginRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

                if (authResponse?.Success == true)
                {
                    await _localStorage.SetItemAsync("authToken", authResponse.Token);
                    await _localStorage.SetItemAsync("user", authResponse.User);

                    var identity = new ClaimsIdentity(ParseClaimsFromJwt(authResponse.Token), "jwt");
                    var user = new ClaimsPrincipal(identity);

                    // Usar el método protegido de la clase base
                    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoginAsync: {ex.Message}");
                return false;
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                await _localStorage.RemoveItemAsync("authToken");
                await _localStorage.RemoveItemAsync("user");

                var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
                // Usar el método protegido de la clase base
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LogoutAsync: {ex.Message}");
            }
        }

        // Método público para notificar cambios de autenticación
        public void NotifyStateChanged(AuthenticationState state)
        {
            NotifyAuthenticationStateChanged(Task.FromResult(state));
        }

        // Sobrecarga del método público para aceptar Task<AuthenticationState>
        public void NotifyStateChanged(Task<AuthenticationState> task)
        {
            base.NotifyAuthenticationStateChanged(task);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            try
            {
                if (string.IsNullOrEmpty(jwt))
                    return Enumerable.Empty<Claim>();

                var parts = jwt.Split('.');
                if (parts.Length != 3)
                    return Enumerable.Empty<Claim>();

                var payload = parts[1];

                // Agregar padding si es necesario
                payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');

                var jsonBytes = Convert.FromBase64String(payload);
                var jsonPayload = Encoding.UTF8.GetString(jsonBytes);

                var keyValuePairs = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonPayload);

                return keyValuePairs?.Select(kvp => new Claim(kvp.Key, kvp.Value?.ToString() ?? ""))
                       ?? Enumerable.Empty<Claim>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing JWT: {ex.Message}");
                Console.WriteLine($"JWT: {jwt}");
                return Enumerable.Empty<Claim>();
            }
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            try
            {
                return await _localStorage.GetItemAsync<User>("user");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCurrentUserAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> IsAdminAsync()
        {
            try
            {
                var user = await GetCurrentUserAsync();
                return user?.Role == UserRole.Admin;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in IsAdminAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<int?> GetCompanyIdAsync()
        {
            try
            {
                var user = await GetCurrentUserAsync();
                return user?.CompanyId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCompanyIdAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> RegisterAsync(User user, string password)
        {
            try
            {
                var registerRequest = new RegisterRequest
                {
                    Username = user.Username,
                    Email = user.Email,
                    Password = password,
                    CompanyId = user.CompanyId
                };

                var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerRequest);
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

                if (authResponse?.Success == true)
                {
                    await _localStorage.SetItemAsync("authToken", authResponse.Token);
                    await _localStorage.SetItemAsync("user", authResponse.User);

                    var identity = new ClaimsIdentity(ParseClaimsFromJwt(authResponse.Token), "jwt");
                    var userPrincipal = new ClaimsPrincipal(identity);

                    // Usar el método protegido de la clase base
                    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(userPrincipal)));
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RegisterAsync: {ex.Message}");
                return false;
            }
        }
    }
}

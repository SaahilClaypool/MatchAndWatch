using System.Net.Http;
using System.Threading.Tasks;

using Blazored.LocalStorage;
using DTO;

using Microsoft.AspNetCore.Components.Authorization;
using DTO.Login;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

// https://chrissainty.com/securing-your-blazor-apps-authentication-with-clientside-blazor-using-webapi-aspnet-core-identity/
namespace Blazor.Helpers {
    public interface IAuthService {
        Task<LoginResultDTO> Login(LoginDTO loginModel);
        Task Logout();
    }
    public class AuthService : IAuthService {
        private readonly HttpClient HttpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider,
                           ILocalStorageService localStorage) {
            HttpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<LoginResultDTO> Login(LoginDTO loginModel) {
            var loginAsJson = JsonSerializer.Serialize(loginModel);
            var response = await HttpClient.PostAsync("api/LogIn", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));
            var loginResult = JsonSerializer.Deserialize<LoginResultDTO>(
                await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true
                })!;

            if (!response.IsSuccessStatusCode) {
                return loginResult;
            }

            await _localStorage.SetItemAsync("authToken", loginResult.Token);
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Username);
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);

            return loginResult;
        }

        public async Task Logout() {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            HttpClient.DefaultRequestHeaders.Authorization = null;
        }
    }

}

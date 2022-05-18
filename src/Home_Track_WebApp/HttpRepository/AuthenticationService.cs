using Blazored.LocalStorage;
using Model.Dto.v1;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Home_Track_WebApp.Shared;

namespace Home_Track_WebApp.HttpRepository
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpClientFactory _IHttpClientFactory;
        private readonly HttpClient _HttpClient;
        private readonly JsonSerializerOptions _options;
        private readonly AuthenticationStateProvider _authStateProvider; 
        private readonly ILocalStorageService _localStorage;


        private readonly IDialogService _DialogService;
        private readonly NavigationManager _navMagager;

        public AuthenticationService(IHttpClientFactory IHttpClientFactory, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage, NavigationManager navManager, IDialogService DialogService)
        {
            _IHttpClientFactory = IHttpClientFactory;
            _HttpClient = _IHttpClientFactory.CreateClient("ApiHomeTrack");
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _authStateProvider = authStateProvider; 
            _localStorage = localStorage;

            _DialogService = DialogService;
            _navMagager = navManager;
        }

        public async Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthentication)
        {
            var content = JsonSerializer.Serialize(userForAuthentication);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            using var response = await _HttpClient.PostAsync("api/v1/accounts/login", bodyContent);

            var authContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<AuthResponseDto>(authContent, _options);

            if (!response.IsSuccessStatusCode)
                return result;

            await _localStorage.SetItemAsync("authToken", result.Token);
            await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);

            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
            
            return new AuthResponseDto { IsAuthSuccessful = true };
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("refreshToken");

            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
            _HttpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<string> RefreshToken()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");

            var tokenDto = JsonSerializer.Serialize(new RefreshTokenDto { Token = token, RefreshToken = refreshToken });
            var bodyContent = new StringContent(tokenDto, Encoding.UTF8, "application/json");

            var refreshResult = await _HttpClient.PostAsync("api/v1/token/refresh", bodyContent);
            var refreshContent = await refreshResult.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AuthResponseDto>(refreshContent, _options);

            if (!refreshResult.IsSuccessStatusCode)
            {
                var Parametro = new DialogParameters { { "Mensaje", "La sesión ha caducado, vuelva a iniciar sesión." } };

                var dialog = _DialogService.Show<DialogoMensaje>(null, Parametro);

                var ResultDialog = await dialog.Result;

                if (!ResultDialog.Cancelled)
                {
                    _navMagager.NavigateTo("/logout");
                }

                return string.Empty;
            }
            else
            {
                await _localStorage.SetItemAsync("authToken", result.Token);
                await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);

                _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

                return result.Token;
            }
        }
    }
}

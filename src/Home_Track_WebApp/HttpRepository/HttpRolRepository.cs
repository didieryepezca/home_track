using Blazored.LocalStorage;
using Microsoft.AspNetCore.WebUtilities;
using Model;
using Model.Dto.v1;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Home_Track_WebApp.HttpRepository
{
    public class HttpRolRepository : IHttpRolRepository
    {
        private readonly IHttpClientFactory _IHttpClientFactory;
        private readonly HttpClient _HttpClient;
        private readonly JsonSerializerOptions _options;
        private readonly ILocalStorageService _localStorage;

        private readonly RefreshTokenService _refreshTokenService;

        public HttpRolRepository(IHttpClientFactory IHttpClientFactory, ILocalStorageService localStorage, RefreshTokenService refreshTokenService)
        {
            _IHttpClientFactory = IHttpClientFactory;
            _HttpClient = _IHttpClientFactory.CreateClient("ApiHomeTrack");
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _localStorage = localStorage;

            _refreshTokenService = refreshTokenService;
        }

        private async Task<string> Token()
        {
            var TokenActualizado = await _refreshTokenService.TryRefreshToken();
            var TokenLocal = await _localStorage.GetItemAsync<string>("authToken");

            return !string.IsNullOrEmpty(TokenActualizado) ? TokenActualizado : TokenLocal;
        }

        public async Task<List<Ent_Rol>> Obten()
        {
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await Token());

            using var response = await _HttpClient.GetAsync("api/v1/Rol");

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<List<Ent_Rol>>(stream, _options);
        }
    }
}
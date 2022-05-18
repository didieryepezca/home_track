using Blazored.LocalStorage;
using Microsoft.AspNetCore.WebUtilities;
using Model;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Home_Track_WebApp.HttpRepository
{
    public class HttpAutorizacionRepository : IHttpAutorizacionRepository
    {
        private readonly IHttpClientFactory _IHttpClientFactory;
        private readonly HttpClient _HttpClient;
        private readonly JsonSerializerOptions _options;
        private readonly ILocalStorageService _localStorage;
        private readonly RefreshTokenService _refreshTokenService;

        public HttpAutorizacionRepository(IHttpClientFactory IHttpClientFactory, ILocalStorageService localStorage, RefreshTokenService refreshTokenService)
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

        public async Task<int> Obten_Cantidad(string Rol_Nombre, string Mod_Nombre, string Ope_Nombre)
        {
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await Token());

            var queryStringParam = new Dictionary<string, string>
            {
                ["Rol_Nombre"] = Rol_Nombre,
                ["Mod_Nombre"] = Mod_Nombre,
                ["Ope_Nombre"] = Ope_Nombre
            };

            using var response = await _HttpClient.GetAsync(QueryHelpers.AddQueryString("api/v1/Autorizacion", queryStringParam));

            var ResponseString = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(ResponseString, _options);
        }
    }
}
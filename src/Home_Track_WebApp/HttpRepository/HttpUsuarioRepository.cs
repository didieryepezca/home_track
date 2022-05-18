using Blazored.LocalStorage;
using Microsoft.AspNetCore.WebUtilities;
using Model;
using Model.Dto.v1;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Home_Track_WebApp.HttpRepository
{
    public class HttpUsuarioRepository : IHttpUsuarioRepository
    {
        private readonly IHttpClientFactory _IHttpClientFactory;
        private readonly HttpClient _HttpClient;
        private readonly JsonSerializerOptions Opciones;
        private readonly ILocalStorageService _localStorage;

        private readonly RefreshTokenService _refreshTokenService;

        public HttpUsuarioRepository(IHttpClientFactory IHttpClientFactory, ILocalStorageService localStorage, RefreshTokenService refreshTokenService)
        {
            _IHttpClientFactory = IHttpClientFactory;
            _HttpClient = _IHttpClientFactory.CreateClient("ApiHomeTrack");
            Opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _localStorage = localStorage;

            _refreshTokenService = refreshTokenService;
        }

        private async Task<string> Token()
        {
            var TokenActualizado = await _refreshTokenService.TryRefreshToken();
            var TokenLocal = await _localStorage.GetItemAsync<string>("authToken");

            return !string.IsNullOrEmpty(TokenActualizado) ? TokenActualizado : TokenLocal;
        }

        public async Task<PagingResponse<Ent_Usuario>> Obten_Paginado(UsuarioParameters Parameters)
        {
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await Token());
            
            var queryStringParam = new Dictionary<string, string>
            {
                ["NumeroPagina"] = Parameters.NumeroPagina.ToString(),
                ["RegistroPagina"] = Parameters.RegistroPagina.ToString(),
                ["PorNombre"] = Parameters.SearchTerm ?? ""
            };

            using var Respuesta = await _HttpClient.GetAsync(QueryHelpers.AddQueryString("api/v1/Usuario", queryStringParam));

            Respuesta.EnsureSuccessStatusCode();

            var Metadata = JsonSerializer.Deserialize<MetaData>(Respuesta.Headers.GetValues("X-Pagination").First(), Opciones);

            var Cuerpo = await Respuesta.Content.ReadAsStreamAsync();

            var pagingResponse = new PagingResponse<Ent_Usuario>
            {
                Cuerpo = await JsonSerializer.DeserializeAsync<List<Ent_Usuario>>(Cuerpo, Opciones),
                MetaData = Metadata
            };

            return pagingResponse;
        }

        public async Task<Ent_Usuario> Obten_x_Id(int Usu_Id)
        {
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await Token());

            using var Respuesta = await _HttpClient.GetAsync($"api/v1/Usuario/{Usu_Id}");

            Respuesta.EnsureSuccessStatusCode();

            var Content = await Respuesta.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Ent_Usuario>(Content, Opciones);
        }

        public async Task<RegistrationResponseDto> Crea(UsuarioNuevoDTO _UsuarioNuevoDTO)
        {
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await Token());

            using var Respuesta = await _HttpClient.PostAsJsonAsync("api/v1/Usuario", _UsuarioNuevoDTO);

            var Cuerpo = await Respuesta.Content.ReadAsStringAsync();

            if (!Respuesta.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<RegistrationResponseDto>(Cuerpo, Opciones);
            }

            return new RegistrationResponseDto { IsSuccessfulRegistration = true, eUsuario = JsonSerializer.Deserialize<Ent_Usuario>(Cuerpo, Opciones) };
        
        }

        public async Task<RegistrationResponseDto> Actualiza(int Usu_Id, UsuarioActualizadoDTO _UsuarioActualizadoDTO)
        {
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await Token());

            var queryStringParam = new Dictionary<string, string>
            {
                ["Usu_Id"] = Usu_Id.ToString()
            };

            using var Respuesta = await _HttpClient.PutAsJsonAsync(QueryHelpers.AddQueryString("api/v1/Usuario", queryStringParam), _UsuarioActualizadoDTO);

            var Cuerpo = await Respuesta.Content.ReadAsStringAsync();

            if (!Respuesta.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<RegistrationResponseDto>(Cuerpo, Opciones);
            }

            return new RegistrationResponseDto { IsSuccessfulRegistration = true };
        }
    }
}
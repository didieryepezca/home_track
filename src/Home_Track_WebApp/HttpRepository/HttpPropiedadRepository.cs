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
    public class HttpPropiedadRepository : IHttpPropiedadRepository
    {
        private readonly IHttpClientFactory _IHttpClientFactory;
        private readonly HttpClient _HttpClient;
        private readonly JsonSerializerOptions Opciones;
        private readonly ILocalStorageService _localStorage;

        private readonly RefreshTokenService _refreshTokenService;

        public HttpPropiedadRepository(IHttpClientFactory IHttpClientFactory, ILocalStorageService localStorage, RefreshTokenService refreshTokenService)
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

        public async Task<PagingResponse<Ent_Propiedad>> Obten_Paginado(PropiedadParameters Parameters)
        {
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await Token());
            
            var queryStringParam = new Dictionary<string, string>
            {
                ["NumeroPagina"] = Parameters.NumeroPagina.ToString(),
                ["RegistroPagina"] = Parameters.RegistroPagina.ToString(),
                ["PorNombre"] = Parameters.SearchTerm ?? ""
            };

            using var Respuesta = await _HttpClient.GetAsync(QueryHelpers.AddQueryString("api/v1/propiedad", queryStringParam));

            Respuesta.EnsureSuccessStatusCode();

            var Metadata = JsonSerializer.Deserialize<MetaData>(Respuesta.Headers.GetValues("X-Pagination").First(), Opciones);

            var Cuerpo = await Respuesta.Content.ReadAsStreamAsync();

            var pagingResponse = new PagingResponse<Ent_Propiedad>
            {
                Cuerpo = await JsonSerializer.DeserializeAsync<List<Ent_Propiedad>>(Cuerpo, Opciones),
                MetaData = Metadata
            };

            return pagingResponse;
        }

        //public async Task<Ent_Propiedad> Obten_x_Id(int Usu_Id)
        //{
        //    _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await Token());

        //    using var Respuesta = await _HttpClient.GetAsync($"api/v1/Propiedad/{Usu_Id}");

        //    Respuesta.EnsureSuccessStatusCode();

        //    var Content = await Respuesta.Content.ReadAsStringAsync();

        //    return JsonSerializer.Deserialize<Ent_Propiedad>(Content, Opciones);
        //}

        //public async Task<RegistrationResponseDto> Crea(PropiedadNuevoDTO _PropiedadNuevoDTO)
        //{
        //    _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await Token());

        //    using var Respuesta = await _HttpClient.PostAsJsonAsync("api/v1/Propiedad", _PropiedadNuevoDTO);

        //    var Cuerpo = await Respuesta.Content.ReadAsStringAsync();

        //    if (!Respuesta.IsSuccessStatusCode)
        //    {
        //        return JsonSerializer.Deserialize<RegistrationResponseDto>(Cuerpo, Opciones);
        //    }

        //    return new RegistrationResponseDto { IsSuccessfulRegistration = true, ePropiedad = JsonSerializer.Deserialize<Ent_Propiedad>(Cuerpo, Opciones) };
        
        //}

        //public async Task<RegistrationResponseDto> Actualiza(int Usu_Id, PropiedadActualizadoDTO _PropiedadActualizadoDTO)
        //{
        //    _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await Token());

        //    var queryStringParam = new Dictionary<string, string>
        //    {
        //        ["Usu_Id"] = Usu_Id.ToString()
        //    };

        //    using var Respuesta = await _HttpClient.PutAsJsonAsync(QueryHelpers.AddQueryString("api/v1/Propiedad", queryStringParam), _PropiedadActualizadoDTO);

        //    var Cuerpo = await Respuesta.Content.ReadAsStringAsync();

        //    if (!Respuesta.IsSuccessStatusCode)
        //    {
        //        return JsonSerializer.Deserialize<RegistrationResponseDto>(Cuerpo, Opciones);
        //    }

        //    return new RegistrationResponseDto { IsSuccessfulRegistration = true };
        //}
    }
}
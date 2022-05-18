using Microsoft.AspNetCore.Components;
using Model;
using MudBlazor;
using Home_Track_WebApp.HttpRepository;
using System.Net.Http.Json;

namespace Home_Track_WebApp.Pages
{
    public partial class Usuario
    {
        [Inject]
        public IHttpUsuarioRepository Repository { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public List<Ent_Usuario> Lst_Usuario { get; set; } = new List<Ent_Usuario>();

        private UsuarioParameters _Parameters = new();

        private readonly int RegistroPagina = 19;
        private int TotalPagina = 0;

        private string searchTerm;

        protected override async Task OnInitializedAsync()
        {
            await Obten_Paginado(RegistroPagina, 1, searchTerm);
        }

        private async Task Obten_Paginado(int RegistroPagina, int NumeroPagina, string PorNombre)
        {
            _Parameters.RegistroPagina = RegistroPagina;
            _Parameters.NumeroPagina = NumeroPagina;
            _Parameters.SearchTerm = PorNombre;

            var response = await Repository.Obten_Paginado(_Parameters);

            Lst_Usuario = response.Cuerpo;
            TotalPagina = response.MetaData.TotalPagina;
        }

        private void Btn_Nuevo_Usuario_Click()
        {
            NavigationManager.NavigateTo("/usuarionuevo");
        }

        private async Task PageChanged(int PaginaSeleccionada)
        {
            await Obten_Paginado(RegistroPagina, PaginaSeleccionada, searchTerm);
        }

        private async Task OnSearch(string PorNombre)
        {
            searchTerm = PorNombre;
            
            await Obten_Paginado(RegistroPagina, 1, searchTerm);
        }
    }
}
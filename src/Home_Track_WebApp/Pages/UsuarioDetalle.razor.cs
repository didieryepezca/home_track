using Microsoft.AspNetCore.Components;
using Model;
using MudBlazor;
using Home_Track_WebApp.HttpRepository;
using System.Net.Http.Json;

namespace Home_Track_WebApp.Pages
{
    public partial class UsuarioDetalle
    {
        public Ent_Usuario oUsuario = new();

        public Ent_Rol oRol = new();

        [Inject]
        public IHttpUsuarioRepository Repository { get; set; }

        [Parameter]
        public int Usu_Id { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected async override Task OnInitializedAsync()
        {
            oUsuario = await Repository.Obten_x_Id(Usu_Id);

            oRol = oUsuario.eRol;
        }

        private void Btn_Actualiza_Usuario_Click()
        {
            NavigationManager.NavigateTo($"/usuarioactualiza/{oUsuario.Usu_Id}");
        }

        private void Btn_Regresar()
        {
            NavigationManager.NavigateTo("/usuario");
        }
    }
}
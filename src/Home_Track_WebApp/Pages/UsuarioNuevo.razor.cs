using Microsoft.AspNetCore.Components;
using Model;
using MudBlazor;
using Home_Track_WebApp.HttpRepository;
using System.Net.Http.Json;
using Model.Dto.v1;
using Home_Track_WebApp.Shared;

namespace Home_Track_WebApp.Pages
{
    public partial class UsuarioNuevo
    {
        public List<Ent_Rol> Lst_Rol { get; set; } = new List<Ent_Rol>();

        public Ent_Rol oRol = new();

        private UsuarioNuevoDTO oUsuarioNuevoDTO = new();

        [Inject]
        public IHttpRolRepository RolRepository { get; set; }

        [Inject]
        public IHttpUsuarioRepository UsuarioRepository { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        private IDialogService DialogService { get; set; }

        private MudForm form;

        private readonly UsuarioNuevoDTOFluentValidator oUsuarioNuevoDTOFluentValidator = new();

        private bool _processing = false;

        public bool ExisteError { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Rol_Obten();
        }

        private async Task Rol_Obten()
        {
            Lst_Rol = await RolRepository.Obten();
        }

        private void Btn_Regresar()
        {
            NavigationManager.NavigateTo("/usuario");
        }

        private async Task Usuario_Registra()
        {
            _processing = true;

            await form.Validate();

            if (form.IsValid)
            {
                var ParametroDecision = new DialogParameters { { "Mensaje", "¿Desea continuar con el Registro?" } };

                var dialog = DialogService.Show<DialogoDecision>(null, ParametroDecision);
                var ResultDialog = await dialog.Result;

                if (!ResultDialog.Cancelled)
                {
                    oUsuarioNuevoDTO.eRol = oRol;

                    var Respuesta = await UsuarioRepository.Crea(oUsuarioNuevoDTO);

                    if (!Respuesta.IsSuccessfulRegistration)
                    {
                        ExisteError = true;

                        var ParametroMensaje = new DialogParameters { { "Mensaje", Respuesta.Errors } };

                        DialogService.Show<DialogoMensaje>(null, ParametroMensaje);
                    }
                    else
                    {
                        NavigationManager.NavigateTo($"/usuariodetalle/{Respuesta.eUsuario.Usu_Id}");
                    }
                }
            }

            _processing = false;
        }
    }
}
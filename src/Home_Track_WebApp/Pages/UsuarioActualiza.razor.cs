using Microsoft.AspNetCore.Components;
using Model;
using MudBlazor;
using Home_Track_WebApp.HttpRepository;
using System.Net.Http.Json;
using Model.Dto.v1;
using Home_Track_WebApp.Shared;

namespace Home_Track_WebApp.Pages
{
    public partial class UsuarioActualiza
    {
        public List<Ent_Rol> Lst_Rol { get; set; } = new List<Ent_Rol>();

        public Ent_Usuario oUsuario = new();

        public Ent_Rol oRol = new();

        private UsuarioActualizadoDTO oUsuarioActualizadoDTO = new();

        [Inject]
        public IHttpRolRepository RolRepository { get; set; }

        [Inject]
        public IHttpUsuarioRepository UsuarioRepository { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        private IDialogService DialogService { get; set; }

        [Parameter]
        public int Usu_Id { get; set; }

        private MudForm form;

        private readonly UsuarioActualizadoDTOFluentValidator oUsuarioActualizadoDTOFluentValidator = new();

        private bool _processing = false;

        public bool ExisteError { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Rol_Obten();

            await Obten_x_Id(Usu_Id);
        }

        private async Task Obten_x_Id(int Usu_Id)
        {
            oUsuario = await UsuarioRepository.Obten_x_Id(Usu_Id);

            oRol = oUsuario.eRol;

            oUsuarioActualizadoDTO.Usu_NumRUT = oUsuario.Usu_NumRUT;
            oUsuarioActualizadoDTO.Usu_NomApeRazSoc = oUsuario.Usu_NomApeRazSoc;
            oUsuarioActualizadoDTO.Usu_NumTelMov = oUsuario.Usu_NumTelMov;
            oUsuarioActualizadoDTO.Usu_Email = oUsuario.Usu_Email;
            oUsuarioActualizadoDTO.Usu_Domicilio = oUsuario.Usu_Domicilio;
            oUsuarioActualizadoDTO.Usu_Observacion = oUsuario.Usu_Observacion;
            oUsuarioActualizadoDTO.eRol = oRol;
        }

        private async Task Rol_Obten()
        {
            Lst_Rol = await RolRepository.Obten();
        }

        private void Btn_Regresar()
        {
            NavigationManager.NavigateTo($"/usuariodetalle/{Usu_Id}");
        }

        private async Task Usuario_Actualiza()
        {
            await form.Validate();

            if (form.IsValid)
            {
                var ParametroDecision = new DialogParameters { { "Mensaje", "¿Desea continuar con la Actualización?" } };

                var dialog = DialogService.Show<DialogoDecision>(null, ParametroDecision);
                var ResultDialog = await dialog.Result;

                if (!ResultDialog.Cancelled)
                {
                    _processing = true;

                    var result = await UsuarioRepository.Actualiza(Usu_Id, oUsuarioActualizadoDTO);

                    _processing = false;

                    if (!result.IsSuccessfulRegistration)
                    {
                        ExisteError = true;

                        var ParametroMensaje = new DialogParameters { { "Mensaje", result.Errors } };

                        DialogService.Show<DialogoMensaje>(null, ParametroMensaje);
                    }
                    else
                    {
                        NavigationManager.NavigateTo($"/usuariodetalle/{Usu_Id}");
                    }
                }
            }
        }
    }
}
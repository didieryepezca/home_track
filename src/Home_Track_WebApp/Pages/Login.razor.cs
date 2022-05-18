using Home_Track_WebApp.HttpRepository;
using Home_Track_WebApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Model.Dto.v1;
using MudBlazor;

namespace Home_Track_WebApp.Pages
{
    public partial class Login
    {
        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        private IDialogService DialogService { get; set; }

        private readonly UserForAuthenticationDto userForAuthentication = new();

        private MudForm form;

        private readonly UserForAuthenticationFluentValidator UserForAuthenticationValidator = new ();

        public string Error { get; set; }

        bool isShow;

        InputType TipoIngresoClave = InputType.Password;

        string TipoIngresoClaveIcono = Icons.Material.Filled.VisibilityOff;

        private bool _processing = false;

        MudButton Boton;

        void ButtonTestclick_Email()
        {
            
        }

        void ButtonTestclick_Clave()
        {
            if(isShow)
            {
                isShow = false;
                TipoIngresoClaveIcono = Icons.Material.Filled.VisibilityOff;
                TipoIngresoClave = InputType.Password;
            }
        else
            {
                isShow = true;
                TipoIngresoClaveIcono = Icons.Material.Filled.Visibility;
                TipoIngresoClave = InputType.Text;
            }
        }

        public async void ExecuteLoginKeyPress(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                await Boton.OnClick.InvokeAsync();
            }
        }

        private async Task InicioSesion()
        {
            _processing = true;

            await form.Validate();

            if (form.IsValid)
            {
                var result = await AuthenticationService.Login(userForAuthentication);

                if (!result.IsAuthSuccessful)
                {
                    var Parametro = new DialogParameters { { "Mensaje", result.ErrorMessage } };

                    DialogService.Show<DialogoMensaje>(null, Parametro);
                }
                else
                {
                    NavigationManager.NavigateTo("/principal");
                }
            }

            _processing = false;
        }
    }
}

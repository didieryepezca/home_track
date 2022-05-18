using Home_Track_WebApp.HttpRepository;
using Microsoft.AspNetCore.Components;

namespace Home_Track_WebApp.Pages
{
    public partial class Logout
    {
        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await AuthenticationService.Logout();
            NavigationManager.NavigateTo("/login");
        }
    }
}

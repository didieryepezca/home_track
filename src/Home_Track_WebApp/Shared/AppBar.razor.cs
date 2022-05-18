using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;

namespace Home_Track_WebApp.Shared
{
    public partial class AppBar
    {
		private bool _isLightMode = true;
		private MudTheme _currentTheme = new();

		[Parameter]
		public EventCallback OnSidebarToggled { get; set; }

		[Parameter]
		public EventCallback<MudTheme> OnThemeToggled { get; set; }

		protected async override Task OnInitializedAsync()
		{
			await ToggleTheme();
		}

		private async Task ToggleTheme()
		{
            _isLightMode = !_isLightMode;

            _currentTheme = !_isLightMode ? GeneratLightTheme() : GenerateDarkTheme();

			await OnThemeToggled.InvokeAsync(_currentTheme);
		}

		private MudTheme GeneratLightTheme() =>
			new()
			{
				Palette = new Palette
				{
                    Primary = "#0c22e7",
					Secondary = "#2a3b47",
					TextPrimary = "#2a3b47",

					Background = "#f8f9fc",

					AppbarBackground = "#0c22e7",
					AppbarText = "#eff3f5",

					DrawerBackground = "#f8f9fc",
					DrawerText = "#2a3b47",
					DrawerIcon = "#2a3b47"
				}
			};

		private MudTheme GenerateDarkTheme() =>
			new()
            {
				Palette = new Palette()
				{
					Primary = "#0c22e7",
					Secondary = "#eff3f5",
					TextPrimary = "#eff3f5",

					Background = "#172129",

					AppbarBackground = "#1e2c36",
					AppbarText = "#eff3f5",

					DrawerBackground = "#172129",
					DrawerText = "#eff3f5",
					DrawerIcon = "#eff3f5",

					Surface = "#1e2c36",
				}
			};
	}
}

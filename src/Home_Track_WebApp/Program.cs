using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Home_Track_WebApp;
using Home_Track_WebApp.HttpRepository;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("ApiHomeTrack", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://localhost:7159/");
    //httpClient.BaseAddress = new Uri("https://hometrackapi.ibrlatam.com:2040");
});

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IHttpUsuarioRepository, HttpUsuarioRepository>();
builder.Services.AddScoped<IHttpPropiedadRepository, HttpPropiedadRepository>();
builder.Services.AddScoped<IHttpRolRepository, HttpRolRepository>();
builder.Services.AddScoped<IHttpAutorizacionRepository, HttpAutorizacionRepository>();
builder.Services.AddScoped<RefreshTokenService>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

builder.Services.AddMudServices();
builder.Services.AddMudBlazorDialog();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
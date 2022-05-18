using Model.Dto.v1;

namespace Home_Track_WebApp.HttpRepository
{
    public interface IAuthenticationService
    {
        Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthentication);

        Task Logout();

        Task<string> RefreshToken();
    }
}

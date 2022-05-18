using Model;
using Model.Dto.v1;

namespace Home_Track_WebApp.HttpRepository
{
    public interface IHttpUsuarioRepository
    {
        Task<PagingResponse<Ent_Usuario>> Obten_Paginado(UsuarioParameters Parameters);

        Task<Ent_Usuario> Obten_x_Id(int Usu_Id);

        Task<RegistrationResponseDto> Crea(UsuarioNuevoDTO _UsuarioNuevoDTO);

        Task<RegistrationResponseDto> Actualiza(int Usu_Id, UsuarioActualizadoDTO _UsuarioActualizadoDTO);
    }
}

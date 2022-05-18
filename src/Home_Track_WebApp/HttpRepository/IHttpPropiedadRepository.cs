using Model;
using Model.Dto.v1;

namespace Home_Track_WebApp.HttpRepository
{
    public interface IHttpPropiedadRepository
    {
        Task<PagingResponse<Ent_Propiedad>> Obten_Paginado(PropiedadParameters Parameters);

        //Task<Ent_Propiedad> Obten_x_Id(int Usu_Id);

        //Task<RegistrationResponseDto> Crea(PropiedadNuevoDTO _PropiedadNuevoDTO);

        //Task<RegistrationResponseDto> Actualiza(int Usu_Id, PropiedadActualizadoDTO _PropiedadActualizadoDTO);
    }
}

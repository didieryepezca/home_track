using AutoMapper;
using Model;
using Model.Dto.v1;

namespace Home_Track_API.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Ent_Usuario, UsuarioDto>();

            CreateMap<Ent_Usuario, UsuarioDto_Obten_x_Id>();

            CreateMap<UsuarioNuevoDTO, Ent_Usuario>();

            CreateMap<Ent_Rol, RolDTO>();

            CreateMap<UsuarioActualizadoDTO, Ent_Usuario>();

            CreateMap<Ent_Tipo_Propiedad, Tipo_PropiedadDTO>();

            CreateMap<Ent_Estado_Propiedad, Estado_PropiedadDTO>();

            CreateMap<Ent_Propiedad, PropiedadDTO>();
        }
    }
}

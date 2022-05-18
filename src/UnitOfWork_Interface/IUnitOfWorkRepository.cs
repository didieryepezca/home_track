using Repository_Interface;

namespace UnitOfWork_Interface
{
    public interface IUnitOfWorkRepository
    {
        IAutorizacionRepository AutorizacionRepository { get; }

        IUsuarioRepository UsuarioRepository { get; }

        IRolRepository RolRepository { get; }

        ITipo_PropiedadRepository Tipo_PropiedadRepository { get; }

        IEstado_PropiedadRepository Estado_PropiedadRepository { get; }

        IPropiedadRepository PropiedadRepository { get; }

        IUsuarioTocRepository UsuarioTocRepository { get; }
    }
}
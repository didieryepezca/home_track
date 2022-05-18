using Repository_Interface;
using Repository_SQLServer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork_Interface;

namespace UnitOfWork_SQLServer
{
    public class UnitOfWorkSQLServerRepository : IUnitOfWorkRepository
    {
        public IAutorizacionRepository AutorizacionRepository { get; }

        public IUsuarioRepository UsuarioRepository { get; }

        public IRolRepository RolRepository { get; }

        public ITipo_PropiedadRepository Tipo_PropiedadRepository { get; }

        public IEstado_PropiedadRepository Estado_PropiedadRepository { get; }

        public IPropiedadRepository PropiedadRepository { get; }

        public IUsuarioTocRepository UsuarioTocRepository  { get; }

    public UnitOfWorkSQLServerRepository(SqlConnection context, SqlTransaction transaction)
        {
            AutorizacionRepository = new AutorizacionRepository(context, transaction);

            UsuarioRepository = new UsuarioRepository(context, transaction);

            RolRepository = new RolRepository(context, transaction);

            Tipo_PropiedadRepository = new Tipo_PropiedadRepository(context, transaction);

            Estado_PropiedadRepository= new Estado_PropiedadRepository(context, transaction);

            PropiedadRepository = new PropiedadRepository(context, transaction);

            UsuarioTocRepository = new UsuarioTocRepository(context, transaction);

        }
    }
}
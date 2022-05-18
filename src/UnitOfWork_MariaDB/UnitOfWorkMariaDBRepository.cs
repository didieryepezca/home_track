using Repository_Interface;
using Repository_MariaDB;
using System;
using System.Collections.Generic;
using MySqlConnector;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork_Interface;

namespace UnitOfWork_MariaDB
{
    public class UnitOfWorkMariaDBRepository : IUnitOfWorkRepository
    {
        public IAdministradorRepository IAdministradorRepository { get; }

        public UnitOfWorkMariaDBRepository(MySqlConnection context, MySqlTransaction transaction)
        {
            IAdministradorRepository = new AdministradorRepository(context, transaction);
        }
    }
}

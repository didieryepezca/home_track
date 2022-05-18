using Model;
using Repository_Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_SQLServer
{
    public class RolRepository : Repository, IRolRepository
    {
        public RolRepository(SqlConnection context, SqlTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }

        public IEnumerable<Ent_Rol> Obten()
        {
            var Lst_Rol = new List<Ent_Rol>();

            using var oCmd = CreateCommand("SP_Rol_Obten");

            oCmd.CommandType = CommandType.StoredProcedure;

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleResult);

            while (oDR.Read())
            {
                Lst_Rol.Add(new Ent_Rol
                {
                    Rol_Nombre = oDR.GetString(oDR.GetOrdinal("Rol_Nombre")),
                });
            }

            return Lst_Rol;
        }
    }
}

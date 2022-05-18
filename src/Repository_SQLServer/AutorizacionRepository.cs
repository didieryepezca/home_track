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
    public class AutorizacionRepository : Repository, IAutorizacionRepository
    {
        public AutorizacionRepository(SqlConnection context, SqlTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }

        public int Obten_Cantidad(string Rol_Nombre, string Mod_Nombre, string Ope_Nombre)
        {
            using var oCmd = CreateCommand("SP_Autorizacion_Obten_Cantidad");

            oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("@Rol_Nombre", Rol_Nombre);
            oCmd.Parameters.AddWithValue("@Mod_Nombre", Mod_Nombre);
            oCmd.Parameters.AddWithValue("@Ope_Nombre", Ope_Nombre);

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleRow);

            oDR.Read();

            return oDR.GetInt32(oDR.GetOrdinal("CANTIDAD"));
        }
    }
}
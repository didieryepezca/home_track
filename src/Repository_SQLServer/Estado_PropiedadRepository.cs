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
    public class Estado_PropiedadRepository : Repository, IEstado_PropiedadRepository
    {
        public Estado_PropiedadRepository(SqlConnection context, SqlTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }

        public IEnumerable<Ent_Estado_Propiedad> Obten()
        {
            var Lst_Estado_Propiedad = new List<Ent_Estado_Propiedad>();

            using var oCmd = CreateCommand("SP_Estado_Propiedad_Obten");

            oCmd.CommandType = CommandType.StoredProcedure;

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleResult);

            while (oDR.Read())
            {
                Lst_Estado_Propiedad.Add(new Ent_Estado_Propiedad
                {
                    EstPro_Nombre = oDR.GetString(oDR.GetOrdinal("EstPro_Nombre")),
                });
            }

            return Lst_Estado_Propiedad;
        }
    }
}

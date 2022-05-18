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
    public class Tipo_PropiedadRepository : Repository, ITipo_PropiedadRepository
    {
        public Tipo_PropiedadRepository(SqlConnection context, SqlTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }

        public IEnumerable<Ent_Tipo_Propiedad> Obten()
        {
            var Lst_Tipo_Propiedad = new List<Ent_Tipo_Propiedad>();

            using var oCmd = CreateCommand("SP_Tipo_Propiedad_Obten");

            oCmd.CommandType = CommandType.StoredProcedure;

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleResult);

            while (oDR.Read())
            {
                Lst_Tipo_Propiedad.Add(new Ent_Tipo_Propiedad
                {
                    TipPro_Nombre = oDR.GetString(oDR.GetOrdinal("TipPro_Nombre")),
                });
            }

            return Lst_Tipo_Propiedad;
        }
    }
}

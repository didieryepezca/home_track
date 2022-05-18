using Model;
using Repository_Interface;
using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_MariaDB
{
    public class AplicacionRepository : Repository, IAplicacionRepository
    {
        public AplicacionRepository(MySqlConnection context, MySqlTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }

        public (int, int, bool, bool, IEnumerable<Ent_Aud_Aplicacion>) Obten_Paginado(int RegistroPagina, int NumeroPagina)
        {
            var Lst_Aud_Aplicacion = new List<Ent_Aud_Aplicacion>();

            using var oCmd = CreateCommand("AUD.SP_AUD_APLICACION_OBTEN_PAGINADO");

            oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("@RegistroPagina", RegistroPagina);
            oCmd.Parameters.AddWithValue("@NumeroPagina", NumeroPagina);
            oCmd.Parameters.AddWithValue("@TotalPagina", 0).Direction = ParameterDirection.Output;
            oCmd.Parameters.AddWithValue("@TotalRegistro", 0).Direction = ParameterDirection.Output;
            oCmd.Parameters.AddWithValue("@TienePaginaAnterior", 0).Direction = ParameterDirection.Output;
            oCmd.Parameters.AddWithValue("@TienePaginaProximo", 0).Direction = ParameterDirection.Output;

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleResult);

            while (oDR.Read())
            {
                Lst_Aud_Aplicacion.Add(new Ent_Aud_Aplicacion
                {
                    Apl_Codigo = oDR.GetInt64(oDR.GetOrdinal("APPI_COD_APLICACION")),
                    Apl_Abreviatura = oDR.GetString(oDR.GetOrdinal("APPV_CODIGO_ABREV")),
                    Apl_Nombre = oDR.GetString(oDR.GetOrdinal("APPV_NOMBRE")),
                    Apl_Descripcion = oDR.GetString(oDR.GetOrdinal("APPV_DESCRIPCION")),
                    Apl_URL = oDR.GetString(oDR.GetOrdinal("APPV_URL")),
                    Apl_Estado = oDR.GetInt32(oDR.GetOrdinal("APPI_EST_REGISTRO")),
                    Apl_Icono = oDR.GetString(oDR.GetOrdinal("APPV_APP_ICONO")),
                    Apl_Color = oDR.GetString(oDR.GetOrdinal("APPV_APP_COLOR"))
                });
            }

            oDR.NextResult();

            return (Convert.ToInt32(oCmd.Parameters["@TotalPagina"].Value),
                    Convert.ToInt32(oCmd.Parameters["@TotalRegistro"].Value),
                    Convert.ToBoolean(oCmd.Parameters["@TienePaginaAnterior"].Value),
                    Convert.ToBoolean(oCmd.Parameters["@TienePaginaProximo"].Value),
                    Lst_Aud_Aplicacion);
        }

        public Ent_Aud_Aplicacion ObtenxId(int Id)
        {
            using var oCmd = CreateCommand("SELECT APPI_COD_APLICACION,APPV_CODIGO_ABREV,APPV_NOMBRE,APPV_DESCRIPCION,APPV_URL,APPI_EST_REGISTRO,APPV_APP_ICONO,APPV_APP_COLOR FROM AUD.AUD_APLICACION WHERE APPI_COD_APLICACION = @Id");

            //oCmd.CommandType = System.Data.CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("@Id", Id);

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleRow);

            if (oDR.HasRows)
            {
                oDR.Read();

                return new Ent_Aud_Aplicacion
                {
                    Apl_Codigo = oDR.GetInt64(oDR.GetOrdinal("APPI_COD_APLICACION")),
                    Apl_Abreviatura = oDR.GetString(oDR.GetOrdinal("APPV_CODIGO_ABREV")),
                    Apl_Nombre = oDR.GetString(oDR.GetOrdinal("APPV_NOMBRE")),
                    Apl_Descripcion = oDR.GetString(oDR.GetOrdinal("APPV_DESCRIPCION")),
                    Apl_URL = oDR.GetString(oDR.GetOrdinal("APPV_URL")),
                    Apl_Estado = oDR.GetInt32(oDR.GetOrdinal("APPI_EST_REGISTRO")),
                    Apl_Icono = oDR.GetString(oDR.GetOrdinal("APPV_APP_ICONO")),
                    Apl_Color = oDR.GetString(oDR.GetOrdinal("APPV_APP_COLOR"))
                };
            }
            else
            {
                return null;
            }
        }

        public void Crea(Ent_Aud_Aplicacion Aplicacion)
        {
            using var oCmd = CreateCommand("INSERT INTO AUD.AUD_APLICACION (APPV_CODIGO_ABREV, APPV_NOMBRE, APPV_DESCRIPCION, APPV_URL, APPI_EST_REGISTRO, APPV_APP_ICONO, APPV_APP_COLOR)" +
                                           "VALUES (@Apl_Abreviatura, @Apl_Nombre, @Apl_Descripcion, @Apl_URL, @Apl_Estado, @Apl_Icono, @Apl_Color)");

            //oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("@Apl_Abreviatura", Aplicacion.Apl_Abreviatura);
            oCmd.Parameters.AddWithValue("@Apl_Nombre", Aplicacion.Apl_Nombre);
            oCmd.Parameters.AddWithValue("@Apl_Descripcion", Aplicacion.Apl_Descripcion);
            oCmd.Parameters.AddWithValue("@Apl_URL", Aplicacion.Apl_URL);
            oCmd.Parameters.AddWithValue("@Apl_Estado", Aplicacion.Apl_Estado);
            oCmd.Parameters.AddWithValue("@Apl_Icono", Aplicacion.Apl_Icono);
            oCmd.Parameters.AddWithValue("@Apl_Color", Aplicacion.Apl_Color);

            oCmd.ExecuteNonQuery();
        }

        public void Actualiza(Ent_Aud_Aplicacion Aplicacion)
        {
            using var oCmd = CreateCommand("UPDATE AUD.AUD_APLICACION SET APPV_CODIGO_ABREV = @Apl_Abreviatura, APPV_NOMBRE = @Apl_Nombre, APPV_DESCRIPCION = @Apl_Descripcion, APPV_URL = @Apl_URL, APPV_APP_ICONO = @Apl_Icono, APPV_APP_COLOR = @Apl_Color WHERE APPI_COD_APLICACION = @Apl_Codigo AND APPI_EST_REGISTRO = 1");

            //oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("@Apl_Codigo", Aplicacion.Apl_Codigo);
            oCmd.Parameters.AddWithValue("@Apl_Abreviatura", Aplicacion.Apl_Abreviatura);
            oCmd.Parameters.AddWithValue("@Apl_Nombre", Aplicacion.Apl_Nombre);
            oCmd.Parameters.AddWithValue("@Apl_Descripcion", Aplicacion.Apl_Descripcion);
            oCmd.Parameters.AddWithValue("@Apl_URL", Aplicacion.Apl_URL);
            oCmd.Parameters.AddWithValue("@Apl_Icono", Aplicacion.Apl_Icono);
            oCmd.Parameters.AddWithValue("@Apl_Color", Aplicacion.Apl_Color);

            oCmd.ExecuteNonQuery();
        }
    }
}

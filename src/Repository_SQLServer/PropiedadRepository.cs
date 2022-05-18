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
    public class PropiedadRepository : Repository, IPropiedadRepository
    {
        public PropiedadRepository(SqlConnection context, SqlTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }

        public (int, int, bool, bool, IEnumerable<Ent_Propiedad>) Obten_Paginado(int RegistroPagina, int NumeroPagina, string PorUbicacion)
        {
            var Lst_Propiedad = new List<Ent_Propiedad>();

            using var oCmd = CreateCommand("SP_Propiedad_Obten_Paginado");

            oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("@RegistroPagina", RegistroPagina);
            oCmd.Parameters.AddWithValue("@NumeroPagina", NumeroPagina);
            oCmd.Parameters.AddWithValue("@PorUbicacion", PorUbicacion);
            oCmd.Parameters.AddWithValue("@TotalPagina", 0).Direction = ParameterDirection.Output;
            oCmd.Parameters.AddWithValue("@TotalRegistro", 0).Direction = ParameterDirection.Output;
            oCmd.Parameters.AddWithValue("@TienePaginaAnterior", 0).Direction = ParameterDirection.Output;
            oCmd.Parameters.AddWithValue("@TienePaginaProximo", 0).Direction = ParameterDirection.Output;

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleResult);

            while (oDR.Read())
            {
                Lst_Propiedad.Add(new Ent_Propiedad
                {
                    Pro_Id = oDR.GetInt32(oDR.GetOrdinal("Pro_Id")),
                    Pro_Ubicacion = oDR.GetString(oDR.GetOrdinal("Pro_Ubicacion")),
                    eTipo_Propiedad = new Ent_Tipo_Propiedad
                    {
                        TipPro_Nombre = oDR.GetString(oDR.GetOrdinal("TipPro_Nombre"))
                    },
                    eEstado_Propiedad = new Ent_Estado_Propiedad
                    {
                        EstPro_Nombre = oDR.GetString(oDR.GetOrdinal("EstPro_Nombre"))
                    },
                    Pro_Valor = oDR.GetDecimal(oDR.GetOrdinal("Pro_Valor")),
                    Pro_FecHorRegistro = oDR.GetDateTime(oDR.GetOrdinal("Pro_FecHorRegistro")),
                    Pro_Condicion = oDR.GetBoolean(oDR.GetOrdinal("Pro_Condicion"))
                });
            }

            oDR.NextResult();

            return (Convert.ToInt32(oCmd.Parameters["@TotalPagina"].Value),
                    Convert.ToInt32(oCmd.Parameters["@TotalRegistro"].Value),
                    Convert.ToBoolean(oCmd.Parameters["@TienePaginaAnterior"].Value),
                    Convert.ToBoolean(oCmd.Parameters["@TienePaginaProximo"].Value),
                    Lst_Propiedad);
        }

        //public Ent_Propiedad Obten_x_Id(int Usu_Id)
        //{
        //    using var oCmd = CreateCommand("SP_Propiedad_Obten_x_Id");

        //    oCmd.CommandType = CommandType.StoredProcedure;

        //    oCmd.Parameters.AddWithValue("@Propiedad_Id", Usu_Id);

        //    using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleRow);

        //    if (oDR.HasRows)
        //    {
        //        oDR.Read();

        //        return new Ent_Propiedad
        //        {
        //            Usu_Id = oDR.GetInt32(oDR.GetOrdinal("Usu_Id")),
        //            Usu_NumRUT = oDR.GetString(oDR.GetOrdinal("Usu_NumRUT")),
        //            Usu_NomApeRazSoc = oDR.GetString(oDR.GetOrdinal("Usu_NomApeRazSoc")),
        //            Usu_NumTelMov = oDR.GetString(oDR.GetOrdinal("Usu_NumTelMov")),
        //            Usu_Email = oDR.GetString(oDR.GetOrdinal("Usu_Email")),
        //            Usu_Domicilio = oDR.GetString(oDR.GetOrdinal("Usu_Domicilio")),
        //            Usu_Observacion = oDR.GetString(oDR.GetOrdinal("Usu_Observacion")),
        //            Usu_FecHorRegistro = oDR.GetDateTime(oDR.GetOrdinal("Usu_FecHorRegistro")),
        //            Usu_Condicion = oDR.GetBoolean(oDR.GetOrdinal("Usu_Condicion")),
        //            eRol = new Ent_Rol
        //            {
        //                Rol_Nombre = oDR.GetString(oDR.GetOrdinal("Rol_Nombre"))
        //            }
        //        };
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public int Existe(string Usu_NumRUT, bool Usu_Condicion)
        //{
        //    using var oCmd = CreateCommand("SP_Propiedad_Existe");

        //    oCmd.CommandType = CommandType.StoredProcedure;
        //    oCmd.Parameters.AddWithValue("@Usu_NumRUT", Usu_NumRUT);
        //    oCmd.Parameters.AddWithValue("@Usu_Condicion", Usu_Condicion);

        //    using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleRow);

        //    oDR.Read();

        //    return oDR.GetInt32(oDR.GetOrdinal("CANTIDAD"));
        //}

        //public int Crea(Ent_Propiedad Propiedad)
        //{
        //    using var oCmd = CreateCommand("SP_Propiedad_Crea");

        //    oCmd.CommandType = CommandType.StoredProcedure;

        //    oCmd.Parameters.AddWithValue("@Usu_NumRUT", Propiedad.Usu_NumRUT);
        //    oCmd.Parameters.AddWithValue("@Usu_NomApeRazSoc", Propiedad.Usu_NomApeRazSoc);
        //    oCmd.Parameters.AddWithValue("@Usu_NumTelMov", Propiedad.Usu_NumTelMov);
        //    oCmd.Parameters.AddWithValue("@Usu_Email", Propiedad.Usu_Email);
        //    oCmd.Parameters.AddWithValue("@Usu_Domicilio", Propiedad.Usu_Domicilio);
        //    oCmd.Parameters.AddWithValue("@Usu_Observacion", Propiedad.Usu_Observacion);
        //    oCmd.Parameters.AddWithValue("@Usu_Clave", Propiedad.Usu_Clave);
        //    oCmd.Parameters.AddWithValue("@Rol_Nombre", Propiedad.eRol.Rol_Nombre);
        //    oCmd.Parameters.AddWithValue("@Usu_Id", 0).Direction = ParameterDirection.Output;

        //    if (oCmd.ExecuteNonQuery() > 0)
        //    {
        //        return (int)oCmd.Parameters["@Usu_Id"].Value;
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}

        //public int Existente(int Usu_Id, string Usu_NumRUT, bool Usu_Condicion)
        //{
        //    using var oCmd = CreateCommand("SP_Propiedad_Existente");

        //    oCmd.CommandType = CommandType.StoredProcedure;

        //    oCmd.Parameters.AddWithValue("@Usu_Id", Usu_Id);
        //    oCmd.Parameters.AddWithValue("@Usu_NumRUT", Usu_NumRUT);
        //    oCmd.Parameters.AddWithValue("@Usu_Condicion", Usu_Condicion);

        //    using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleRow);

        //    oDR.Read();

        //    return oDR.GetInt32(oDR.GetOrdinal("CANTIDAD"));
        //}

        //public int Actualiza(Ent_Propiedad Propiedad)
        //{
        //    using var oCmd = CreateCommand("SP_Propiedad_Actualiza");

        //    oCmd.CommandType = CommandType.StoredProcedure;

        //    oCmd.Parameters.AddWithValue("@Usu_Id", Propiedad.Usu_Id);
        //    oCmd.Parameters.AddWithValue("@Usu_NumRUT", Propiedad.Usu_NumRUT);
        //    oCmd.Parameters.AddWithValue("@Usu_NomApeRazSoc", Propiedad.Usu_NomApeRazSoc);
        //    oCmd.Parameters.AddWithValue("@Usu_NumTelMov", Propiedad.Usu_NumTelMov);
        //    oCmd.Parameters.AddWithValue("@Usu_Email", Propiedad.Usu_Email);
        //    oCmd.Parameters.AddWithValue("@Usu_Domicilio", Propiedad.Usu_Domicilio);
        //    oCmd.Parameters.AddWithValue("@Usu_Observacion", Propiedad.Usu_Observacion);
        //    oCmd.Parameters.AddWithValue("@Rol_Nombre", Propiedad.eRol.Rol_Nombre);

        //    return oCmd.ExecuteNonQuery();
        //}
    }
}

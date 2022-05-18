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
    public class UsuarioRepository : Repository, IUsuarioRepository
    {
        public UsuarioRepository(SqlConnection context, SqlTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }

        public (int, int, bool, bool, IEnumerable<Ent_Usuario>) Obten_Paginado(int RegistroPagina, int NumeroPagina, string PorNombre)
        {
            var Lst_Usuario = new List<Ent_Usuario>();

            using var oCmd = CreateCommand("SP_Usuario_Obten_Paginado");

            oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("@RegistroPagina", RegistroPagina);
            oCmd.Parameters.AddWithValue("@NumeroPagina", NumeroPagina);
            oCmd.Parameters.AddWithValue("@PorNombre", PorNombre);
            oCmd.Parameters.AddWithValue("@TotalPagina", 0).Direction = ParameterDirection.Output;
            oCmd.Parameters.AddWithValue("@TotalRegistro", 0).Direction = ParameterDirection.Output;
            oCmd.Parameters.AddWithValue("@TienePaginaAnterior", 0).Direction = ParameterDirection.Output;
            oCmd.Parameters.AddWithValue("@TienePaginaProximo", 0).Direction = ParameterDirection.Output;

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleResult);

            while (oDR.Read())
            {
                Lst_Usuario.Add(new Ent_Usuario
                {
                    Usu_Id = oDR.GetInt32(oDR.GetOrdinal("Usu_Id")),
                    Usu_NumRUT = oDR.GetString(oDR.GetOrdinal("Usu_NumRUT")),
                    Usu_NomApeRazSoc = oDR.GetString(oDR.GetOrdinal("Usu_NomApeRazSoc")),
                    Usu_NumTelMov = oDR.GetString(oDR.GetOrdinal("Usu_NumTelMov")),
                    Usu_Email = oDR.GetString(oDR.GetOrdinal("Usu_Email")),
                    eRol = new Ent_Rol
                    {
                        Rol_Nombre = oDR.GetString(oDR.GetOrdinal("Rol_Nombre"))
                    },
                    Usu_FecHorRegistro = oDR.GetDateTime(oDR.GetOrdinal("Usu_FecHorRegistro")),
                    Usu_Condicion = oDR.GetBoolean(oDR.GetOrdinal("Usu_Condicion"))
                });
            }

            oDR.NextResult();

            return (Convert.ToInt32(oCmd.Parameters["@TotalPagina"].Value),
                    Convert.ToInt32(oCmd.Parameters["@TotalRegistro"].Value),
                    Convert.ToBoolean(oCmd.Parameters["@TienePaginaAnterior"].Value),
                    Convert.ToBoolean(oCmd.Parameters["@TienePaginaProximo"].Value),
                    Lst_Usuario);
        }

        public int Obten_Login(string Usu_Email, string Usu_Clave)
        {
            using var oCmd = CreateCommand("SP_Usuario_Obten_Login");

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.Parameters.AddWithValue("@Usu_Email", Usu_Email);
            oCmd.Parameters.AddWithValue("@Usu_Clave", Usu_Clave);

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleRow);

            oDR.Read();

            return oDR.GetInt32(oDR.GetOrdinal("CANTIDAD"));
        }

        public Ent_Usuario Obten_x_Id(int Usu_Id)
        {
            using var oCmd = CreateCommand("SP_Usuario_Obten_x_Id");

            oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("@Usuario_Id", Usu_Id);

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleRow);

            if (oDR.HasRows)
            {
                oDR.Read();

                return new Ent_Usuario
                {
                    Usu_Id = oDR.GetInt32(oDR.GetOrdinal("Usu_Id")),
                    Usu_NumRUT = oDR.GetString(oDR.GetOrdinal("Usu_NumRUT")),
                    Usu_NomApeRazSoc = oDR.GetString(oDR.GetOrdinal("Usu_NomApeRazSoc")),
                    Usu_NumTelMov = oDR.GetString(oDR.GetOrdinal("Usu_NumTelMov")),
                    Usu_Email = oDR.GetString(oDR.GetOrdinal("Usu_Email")),
                    Usu_Domicilio = oDR.GetString(oDR.GetOrdinal("Usu_Domicilio")),
                    Usu_Observacion = !oDR.IsDBNull(oDR.GetOrdinal("Usu_Observacion")) ? oDR.GetString(oDR.GetOrdinal("Usu_Observacion")) : string.Empty,
                    Usu_FecHorRegistro = oDR.GetDateTime(oDR.GetOrdinal("Usu_FecHorRegistro")),
                    Usu_Condicion = oDR.GetBoolean(oDR.GetOrdinal("Usu_Condicion")),
                    eRol = new Ent_Rol
                    {
                        Rol_Nombre = oDR.GetString(oDR.GetOrdinal("Rol_Nombre"))
                    }
                };
            }
            else
            {
                return null;
            }
        }

        public Ent_Usuario Obten_x_Email(string Usu_Email)
        {
            using var oCmd = CreateCommand("SP_Usuario_Obten_x_Email");

            oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("@Usu_Email", Usu_Email);

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleRow);

            if (oDR.HasRows)
            {
                oDR.Read();

                return new Ent_Usuario
                {
                    Usu_Email = oDR.GetString(oDR.GetOrdinal("Usu_Email")),
                    Usu_TokenActualizado = oDR.GetString(oDR.GetOrdinal("Usu_TokenActualizado")) != null ? oDR.GetString(oDR.GetOrdinal("Usu_TokenActualizado")) : string.Empty,
                    Usu_FecHorTokenActualizado = oDR.GetDateTime(oDR.GetOrdinal("Usu_FecHorTokenActualizado")),
                    eRol = new Ent_Rol
                    {
                        Rol_Nombre= oDR.GetString(oDR.GetOrdinal("Rol_Nombre"))
                    }
                };
            }
            else
            {
                return null;
            }
        }

        public bool Actualiza_Token(Ent_Usuario Usuario)
        {
            using var oCmd = CreateCommand("SP_Usuario_ActualizaToken");

            oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("@Usu_TokenActualizado", Usuario.Usu_TokenActualizado);
            oCmd.Parameters.AddWithValue("@Usu_FecHorTokenActualizado", Usuario.Usu_FecHorTokenActualizado);
            oCmd.Parameters.AddWithValue("@Usu_Email", Usuario.Usu_Email);

            return oCmd.ExecuteNonQuery() > 0 ? true : false;
        }

        public int Existe(string Usu_NumRUT, bool Usu_Condicion)
        {
            using var oCmd = CreateCommand("SP_Usuario_Existe");

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.Parameters.AddWithValue("@Usu_NumRUT", Usu_NumRUT);
            oCmd.Parameters.AddWithValue("@Usu_Condicion", Usu_Condicion);

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleRow);

            oDR.Read();

            return oDR.GetInt32(oDR.GetOrdinal("CANTIDAD"));
        }

        public int Crea(Ent_Usuario Usuario)
        {
            using var oCmd = CreateCommand("SP_Usuario_Crea");

            oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("@Usu_NumRUT", Usuario.Usu_NumRUT);
            oCmd.Parameters.AddWithValue("@Usu_NomApeRazSoc", Usuario.Usu_NomApeRazSoc);
            oCmd.Parameters.AddWithValue("@Usu_NumTelMov", Usuario.Usu_NumTelMov);
            oCmd.Parameters.AddWithValue("@Usu_Email", Usuario.Usu_Email);
            oCmd.Parameters.AddWithValue("@Usu_Domicilio", Usuario.Usu_Domicilio);
            oCmd.Parameters.AddWithValue("@Usu_Observacion", Usuario.Usu_Observacion);
            oCmd.Parameters.AddWithValue("@Usu_Clave", Usuario.Usu_Clave);
            oCmd.Parameters.AddWithValue("@Rol_Nombre", Usuario.eRol.Rol_Nombre);
            oCmd.Parameters.AddWithValue("@Usu_Id", 0).Direction = ParameterDirection.Output;

            if (oCmd.ExecuteNonQuery() > 0)
            {
                return (int)oCmd.Parameters["@Usu_Id"].Value;
            }
            else
            {
                return 0;
            }
        }

        public int Existente(int Usu_Id, string Usu_NumRUT, bool Usu_Condicion)
        {
            using var oCmd = CreateCommand("SP_Usuario_Existente");

            oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("@Usu_Id", Usu_Id);
            oCmd.Parameters.AddWithValue("@Usu_NumRUT", Usu_NumRUT);
            oCmd.Parameters.AddWithValue("@Usu_Condicion", Usu_Condicion);

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleRow);

            oDR.Read();

            return oDR.GetInt32(oDR.GetOrdinal("CANTIDAD"));
        }

        public int Actualiza(Ent_Usuario Usuario)
        {
            using var oCmd = CreateCommand("SP_Usuario_Actualiza");

            oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("@Usu_Id", Usuario.Usu_Id);
            oCmd.Parameters.AddWithValue("@Usu_NumRUT", Usuario.Usu_NumRUT);
            oCmd.Parameters.AddWithValue("@Usu_NomApeRazSoc", Usuario.Usu_NomApeRazSoc);
            oCmd.Parameters.AddWithValue("@Usu_NumTelMov", Usuario.Usu_NumTelMov);
            oCmd.Parameters.AddWithValue("@Usu_Email", Usuario.Usu_Email);
            oCmd.Parameters.AddWithValue("@Usu_Domicilio", Usuario.Usu_Domicilio);
            oCmd.Parameters.AddWithValue("@Usu_Observacion", Usuario.Usu_Observacion);
            oCmd.Parameters.AddWithValue("@Rol_Nombre", Usuario.eRol.Rol_Nombre);

            return oCmd.ExecuteNonQuery();
        }
    }
}

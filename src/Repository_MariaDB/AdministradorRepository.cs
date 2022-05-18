using Model;
using MySqlConnector;
using Repository_Interface;
using System;
using System.Collections.Generic;
using System.Data;

namespace Repository_MariaDB
{
    public class AdministradorRepository : Repository, IAdministradorRepository
    {
        public AdministradorRepository(MySqlConnection context, MySqlTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }

        public IEnumerable<Ent_Administrador> Obten_Paginado(int Opcion, string Valor, bool Condicion, int Inicio, int Fin)
        {
            var result = new List<Ent_Administrador>();

            using (var oCmd = CreateCommand("SP_Administrador_Obten_Paginado"))
            {
                oCmd.CommandType = CommandType.StoredProcedure;

                oCmd.Parameters.AddWithValue("Opcion", Opcion);
                oCmd.Parameters.AddWithValue("Valor", Valor);
                oCmd.Parameters.AddWithValue("Condicion", Condicion);
                oCmd.Parameters.AddWithValue("Inicio", Inicio);
                oCmd.Parameters.AddWithValue("Fin", Fin);

                using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleResult);

                while (oDR.Read())
                {
                    result.Add(new Ent_Administrador
                    {
                        Adm_Id = oDR.GetInt32(oDR.GetOrdinal("Adm_Id")),
                        Adm_RUT = oDR.GetString(oDR.GetOrdinal("Adm_RUT")),
                        Adm_Apellidos = oDR.GetString(oDR.GetOrdinal("Adm_Apellidos")),
                        Adm_PreNombres = oDR.GetString(oDR.GetOrdinal("Adm_PreNombres")),
                        Adm_NumTelMov = oDR.GetString(oDR.GetOrdinal("Adm_NumTelMov")),
                        Adm_Email = oDR.GetString(oDR.GetOrdinal("Adm_Email")),
                        Adm_FecHorRegistro = oDR.GetDateTime(oDR.GetOrdinal("Adm_FecHorRegistro")),
                        Adm_Condicion = oDR.GetBoolean(oDR.GetOrdinal("Adm_Condicion"))
                    });
                }
            }

            return result;
        }

        public int Obten_Paginado_Cantidad(int Opcion, string Valor, bool Condicion)
        {
            using var oCmd = CreateCommand("SP_Administrador_Obten_Paginado_Cantidad");

            oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("Opcion", Opcion);
            oCmd.Parameters.AddWithValue("Valor", Valor);
            oCmd.Parameters.AddWithValue("Condicion", Condicion);

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleRow);

            if (oDR.HasRows)
            {
                oDR.Read();

                return oDR.GetInt32(oDR.GetOrdinal("CANTIDAD"));
            }
            else
            {
                return 0;
            }
        }

        public int Obten_Cantidad()
        {
            using var oCmd = CreateCommand("SP_Administrador_Obten_Cantidad");

            oCmd.CommandType = CommandType.StoredProcedure;

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleRow);

            if (oDR.HasRows)
            {
                oDR.Read();

                return oDR.GetInt32(oDR.GetOrdinal("CANTIDAD"));
            }
            else
            {
                return 0;
            }
        }

        public byte[] Obten_Pass(string Adm_NumDNI)
        {
            using var oCmd = CreateCommand("SP_Administrador_Obten_Pass");

            oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("Adm_NumDNI", Adm_NumDNI);

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleRow);

            if (oDR.HasRows)
            {
                oDR.Read();

                return (byte[])oDR.GetValue(oDR.GetOrdinal("Adm_Pass"));
            }
            else
            {
                return null;
            }
        }

        public byte[] Obten_Salt(string Adm_NumDNI)
        {
            using var oCmd = CreateCommand("SP_Administrador_Obten_Salt");

            oCmd.CommandType = CommandType.StoredProcedure;
            
            oCmd.Parameters.AddWithValue("Adm_NumDNI", Adm_NumDNI);

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleRow);

            if (oDR.HasRows)
            {
                oDR.Read();

                return (byte[])oDR.GetValue(oDR.GetOrdinal("Adm_Salt"));
            }
            else
            {
                return null;
            }
        }

        public int Obten_Login(string Adm_NumDNI, byte[] Adm_Salt, byte[] Adm_Pass)
        {
            using var oCmd = CreateCommand("SP_Administrador_Obten_Login");

            oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("Adm_NumDNI", Adm_NumDNI);
            oCmd.Parameters.AddWithValue("Adm_Salt", Adm_Salt);
            oCmd.Parameters.AddWithValue("Adm_Pass", Adm_Pass);

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleRow);

            if (oDR.HasRows)
            {
                oDR.Read();

                return oDR.GetInt32(oDR.GetOrdinal("CANTIDAD"));
            }
            else
            {
                return 0;
            }
        }

        public int Obten_Login(string Adm_Email, string Adm_Contra)
        {
            using var oCmd = CreateCommand("SP_Administrador_Obten_Login");

            oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("Adm_Email", Adm_Email);
            oCmd.Parameters.AddWithValue("Adm_Contra", Adm_Contra);

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleRow);

            if (oDR.HasRows)
            {
                oDR.Read();

                return oDR.GetInt32(oDR.GetOrdinal("Adm_Id"));
            }
            else
            {
                return 0;
            }
        }

        public (int, string, string) Obten_Nombre_Completo(string Adm_NumDNI)
        {
            using var oCmd = CreateCommand("SP_Administrador_Obten_Nombre_Completo");

            oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("Adm_NumDNI", Adm_NumDNI);

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleRow);

            if (oDR.HasRows)
            {
                oDR.Read();

                return (oDR.GetInt32(oDR.GetOrdinal("Adm_Id")), oDR.GetString(oDR.GetOrdinal("Adm_Apellidos")), oDR.GetString(oDR.GetOrdinal("Adm_PreNombres")));
            }
            else
            {
                return (0,string.Empty, string.Empty);
            }
        }

        public Ent_Administrador Obten_x_Email(string Adm_Email)
        {
            using var oCmd = CreateCommand("SP_Administrador_Obten_x_Email");

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("Adm_Email", Adm_Email);

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleRow);

            oDR.Read();

            return new Ent_Administrador
            {
                Adm_Id = oDR.GetInt32(oDR.GetOrdinal("Adm_Id")),
                Adm_RUT = oDR.GetString(oDR.GetOrdinal("Adm_RUT")),
                Adm_Apellidos = oDR.GetString(oDR.GetOrdinal("Adm_Apellidos")),
                Adm_PreNombres = oDR.GetString(oDR.GetOrdinal("Adm_PreNombres")),
                Adm_NumTelMov = oDR.GetString(oDR.GetOrdinal("Adm_NumTelMov")),
                Adm_Email = oDR.GetString(oDR.GetOrdinal("Adm_Email")),
                Adm_Domicilio = oDR.GetString(oDR.GetOrdinal("Adm_Domicilio")),
                //Adm_Salt = (byte[])oDR.GetValue(oDR.GetOrdinal("Adm_Salt")),
                //Adm_Pass = (byte[])oDR.GetValue(oDR.GetOrdinal("Adm_Pass")),
                Adm_RefPass = oDR.GetString(oDR.GetOrdinal("Adm_RefPass")),
                Adm_Observacion = oDR.GetString(oDR.GetOrdinal("Adm_Observacion")),
                Adm_FecHorRegistro = oDR.GetDateTime(oDR.GetOrdinal("Adm_FecHorRegistro")),
                Adm_Condicion = oDR.GetBoolean(oDR.GetOrdinal("Adm_Condicion"))
            };
        }

        public Ent_Administrador Obten(int Adm_Id)
        {
            using var oCmd = CreateCommand("SP_Administrador_Obten");

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("Adm_Id", Adm_Id);

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleRow);

            oDR.Read();

            return new Ent_Administrador
            {
                Adm_Id = oDR.GetInt32(oDR.GetOrdinal("Adm_Id")),
                Adm_RUT = oDR.GetString(oDR.GetOrdinal("Adm_RUT")),
                Adm_Apellidos = oDR.GetString(oDR.GetOrdinal("Adm_Apellidos")),
                Adm_PreNombres = oDR.GetString(oDR.GetOrdinal("Adm_PreNombres")),
                Adm_NumTelMov = oDR.GetString(oDR.GetOrdinal("Adm_NumTelMov")),
                Adm_Email = oDR.GetString(oDR.GetOrdinal("Adm_Email")),
                Adm_Domicilio = oDR.GetString(oDR.GetOrdinal("Adm_Domicilio")),
                //Adm_Salt = (byte[])oDR.GetValue(oDR.GetOrdinal("Adm_Salt")),
                //Adm_Pass = (byte[])oDR.GetValue(oDR.GetOrdinal("Adm_Pass")),
                Adm_RefPass = oDR.GetString(oDR.GetOrdinal("Adm_RefPass")),
                Adm_Observacion = oDR.GetString(oDR.GetOrdinal("Adm_Observacion")),
                Adm_FecHorRegistro = oDR.GetDateTime(oDR.GetOrdinal("Adm_FecHorRegistro")),
                Adm_Condicion = oDR.GetBoolean(oDR.GetOrdinal("Adm_Condicion"))
            };
        }

        public int Obten_Cantidad_Existe(Ent_Administrador Ent_Administrador)
        {
            using var oCmd = CreateCommand("SP_Administrador_Obten_Cantidad_Existe");

            oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("Adm_RUT", Ent_Administrador.Adm_RUT);
            oCmd.Parameters.AddWithValue("Adm_Condicion", Ent_Administrador.Adm_Condicion);

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleRow);

            if (oDR.HasRows)
            {
                oDR.Read();

                return oDR.GetInt32(oDR.GetOrdinal("CANTIDAD"));
            }
            else
            {
                return 0;
            }
        }

        public int Crea(Ent_Administrador Administrador)
        {
            using var oCmd = CreateCommand("SP_Administrador_Crea");

            oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("Adm_RUT", Administrador.Adm_RUT);
            oCmd.Parameters.AddWithValue("Adm_Apellidos", Administrador.Adm_Apellidos);
            oCmd.Parameters.AddWithValue("Adm_PreNombres", Administrador.Adm_PreNombres);
            oCmd.Parameters.AddWithValue("Adm_NumTelMov", Administrador.Adm_NumTelMov);
            oCmd.Parameters.AddWithValue("Adm_Email", Administrador.Adm_Email);
            oCmd.Parameters.AddWithValue("Adm_Domicilio", Administrador.Adm_Domicilio);
            oCmd.Parameters.AddWithValue("Adm_Salt", Administrador.Adm_Salt);
            oCmd.Parameters.AddWithValue("Adm_Pass", Administrador.Adm_Pass);
            oCmd.Parameters.AddWithValue("Adm_RefPass", Administrador.Adm_RefPass);
            oCmd.Parameters.AddWithValue("Adm_Observacion", Administrador.Adm_Observacion);

            return oCmd.ExecuteNonQuery();
        }

        public int Obten_Cantidad_Existente(Ent_Administrador Administrador)
        {
            using var oCmd = CreateCommand("SP_Administrador_Cantidad_Existente");

            oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("Adm_Id", Administrador.Adm_Id);
            oCmd.Parameters.AddWithValue("Adm_RUT", Administrador.Adm_RUT);
            oCmd.Parameters.AddWithValue("Adm_Condicion", Administrador.Adm_Condicion);

            return Convert.ToInt32(oCmd.ExecuteScalar());
        }

        public int Actualiza(Ent_Administrador Administrador)
        {
            using var oCmd = CreateCommand("SP_Administrador_Actualiza");

            oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("Adm_Id", Administrador.Adm_Id);
            oCmd.Parameters.AddWithValue("Adm_Apellidos", Administrador.Adm_Apellidos);
            oCmd.Parameters.AddWithValue("Adm_PreNombres", Administrador.Adm_PreNombres);
            oCmd.Parameters.AddWithValue("Adm_NumTelMov", Administrador.Adm_NumTelMov);
            oCmd.Parameters.AddWithValue("Adm_Email", Administrador.Adm_Email);
            oCmd.Parameters.AddWithValue("Adm_Domicilio", Administrador.Adm_Domicilio);
            oCmd.Parameters.AddWithValue("Adm_Observacion", Administrador.Adm_Observacion);

            return oCmd.ExecuteNonQuery();
        }

        public int Actualiza_Contrasenia(Ent_Administrador Administrador)
        {
            using var oCmd = CreateCommand("SP_Administrador_Actualiza_Contrasenia");

            oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("Adm_RUT", Administrador.Adm_RUT);
            oCmd.Parameters.AddWithValue("Adm_Salt", Administrador.Adm_Salt);
            oCmd.Parameters.AddWithValue("Adm_Pass", Administrador.Adm_Pass);

            return oCmd.ExecuteNonQuery();
        }

        public int Actualiza_Condicion(Ent_Administrador Administrador)
        {
            using var oCmd = CreateCommand("SP_Administrador_Actualiza_Condicion");

            oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("Adm_Id", Administrador.Adm_Id);
            oCmd.Parameters.AddWithValue("Adm_Observacion", Administrador.Adm_Observacion);
            oCmd.Parameters.AddWithValue("Adm_Condicion", Administrador.Adm_Condicion);

            return oCmd.ExecuteNonQuery();
        }
    }
}

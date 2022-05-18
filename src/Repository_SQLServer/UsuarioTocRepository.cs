using Model;
using Model.Entitie;
using Repository_Interface;
using System.Data;
using System.Data.SqlClient;

namespace Repository_SQLServer
{
    public class UsuarioTocRepository : Repository, IUsuarioTocRepository
    {
        public UsuarioTocRepository(SqlConnection context, SqlTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }

        public int CreaTocToken(Ent_Usu_Toc_Token UsuTocToken)
        {
            using var oCmd = CreateCommand("SP_Usuario_TocToken_Crea");

            oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("@UsuTocToken_NumRUT", UsuTocToken.UsuTocTok_Rut);
            oCmd.Parameters.AddWithValue("@UsuTocToken_Token", UsuTocToken.UsuTocTok_Token);
            oCmd.Parameters.AddWithValue("@UsuTocToken_Fecha_Reg", UsuTocToken.UsuTocTok_Fecha_Reg);
            oCmd.Parameters.AddWithValue("@UsuTocToken_Condicion", UsuTocToken.UsuTocTok_Condicion);            
            
            oCmd.Parameters.AddWithValue("@UsuTocToken_Id", 0).Direction = ParameterDirection.Output;

            if (oCmd.ExecuteNonQuery() > 0)
            {
                return (int)oCmd.Parameters["@UsuTocToken_Id"].Value;
            }
            else
            {
                return 0;
            }
        }

        public Ent_Usu_Toc_Token ObtenTocToken_x_Ruc(string Usu_Ruc, bool UsuTocTok_Condicion)
        {
            using var oCmd = CreateCommand("SP_Usuario_TocToken_Obten_x_Ruc");

            oCmd.CommandType = CommandType.StoredProcedure;

            oCmd.Parameters.AddWithValue("@Usuario_Ruc", Usu_Ruc);
            oCmd.Parameters.AddWithValue("@UsuTocTok_Condicion", UsuTocTok_Condicion);

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleRow);

            if (oDR.HasRows)
            {
                oDR.Read();

                return new Ent_Usu_Toc_Token
                {
                    UsuTocTok_Id = oDR.GetInt32(oDR.GetOrdinal("UsuTocTok_Id")),
                    UsuTocTok_Rut = oDR.GetString(oDR.GetOrdinal("UsuTocTok_NumRUT")),
                    UsuTocTok_Token = oDR.GetString(oDR.GetOrdinal("UsuTocTok_Token")),
                    UsuTocTok_Fecha_Reg = oDR.GetDateTime(oDR.GetOrdinal("UsuTocTok_Fecha_Reg")),
                    UsuTocTok_Condicion = oDR.GetBoolean(oDR.GetOrdinal("UsuTocTok_Condicion")),
                };
            }
            else
            {
                return null;
            }
        }

        public int TocTokenExistente(string UsuTocTok_NumRUT, bool UsuTocTok_Condicion)
        {
            using var oCmd = CreateCommand("SP_Usuario_TocToken_Existente");

            oCmd.CommandType = CommandType.StoredProcedure;
            
            oCmd.Parameters.AddWithValue("@UsuTocTok_NumRUT", UsuTocTok_NumRUT);
            oCmd.Parameters.AddWithValue("@UsuTocTok_Condicion", UsuTocTok_Condicion);

            using var oDR = oCmd.ExecuteReader(CommandBehavior.SingleRow);

            oDR.Read();

            return oDR.GetInt32(oDR.GetOrdinal("CANTIDAD_TOC_TOKEN"));
        }
    }
}

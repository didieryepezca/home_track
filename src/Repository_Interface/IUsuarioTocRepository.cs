using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Entitie;

namespace Repository_Interface
{
    public interface IUsuarioTocRepository
    {
        int CreaTocToken(Ent_Usu_Toc_Token UsuTocToken);

        Ent_Usu_Toc_Token ObtenTocToken_x_Ruc(string Usu_Ruc, bool Usu_Condicion);

        int TocTokenExistente(string Usu_NumRUT, bool Usu_Condicion);

    }
}

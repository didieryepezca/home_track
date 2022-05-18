using Model;
using Repository_Interface.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository_Interface
{
    public interface IUsuarioRepository //: IReadRepository<Ent_Administrador, int, string, bool, int, int>, ICreateRepository<Ent_Administrador>, IUpdateRepository<Ent_Administrador>
    {
        (int, int, bool, bool, IEnumerable<Ent_Usuario>) Obten_Paginado(int RegistroPagina, int NumeroPagina, string PorNombre);

        Ent_Usuario Obten_x_Id(int Usu_Id);

        Ent_Usuario Obten_x_Email(string Adm_Email);

        //byte[] Obten_Salt(string Adm_NumDNI);

        //byte[] Obten_Pass(string Adm_NumDNI);

        //int Obten_Login(string Adm_NumDNI, byte[] Adm_Salt, byte[] Adm_Pass);

        int Obten_Login(string Adm_Email, string Adm_Contra);

        int Existe(string Usu_NumRUT, bool Usu_Condicion);

        //(int, string, string) Obten_Nombre_Completo(string Adm_NumDNI);

        int Crea(Ent_Usuario Usuario);

        int Existente(int Usu_Id, string Usu_NumRUT, bool Usu_Condicion);

        int Actualiza(Ent_Usuario Usuario);

        bool Actualiza_Token(Ent_Usuario Usuario);
    }
}

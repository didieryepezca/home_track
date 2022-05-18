using Model;
using Repository_Interface.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository_Interface
{
    public interface IPropiedadRepository
    {
        (int, int, bool, bool, IEnumerable<Ent_Propiedad>) Obten_Paginado(int RegistroPagina, int NumeroPagina, string PorUbicacion);

        //Ent_Propiedad Obten_x_Id(int Usu_Id);

        //int Existe(string Usu_NumRUT, bool Usu_Condicion);

        //int Crea(Ent_Propiedad Propiedad);

        //int Existente(int Usu_Id, string Usu_NumRUT, bool Usu_Condicion);

        //int Actualiza(Ent_Propiedad Propiedad);
    }
}

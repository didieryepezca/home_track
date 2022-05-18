using Model;
using Repository_Interface.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository_Interface
{
    public interface IAutorizacionRepository
    {
        int Obten_Cantidad(string Rol_Nombre, string Mod_Nombre, string Ope_Nombre);
    }
}

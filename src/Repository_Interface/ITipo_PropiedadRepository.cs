using Model;
using Repository_Interface.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository_Interface
{
    public interface ITipo_PropiedadRepository
    {
        IEnumerable<Ent_Tipo_Propiedad> Obten();
    }
}
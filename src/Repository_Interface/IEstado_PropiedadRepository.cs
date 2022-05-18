using Model;
using Repository_Interface.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository_Interface
{
    public interface IEstado_PropiedadRepository
    {
        IEnumerable<Ent_Estado_Propiedad> Obten();
    }
}
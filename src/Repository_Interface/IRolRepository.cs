using Model;
using Repository_Interface.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository_Interface
{
    public interface IRolRepository
    {
        IEnumerable<Ent_Rol> Obten();
    }
}
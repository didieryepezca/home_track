using System;

namespace Model
{
    public class Ent_Autorizacion
    {
        public int Aut_Id { get; set; }

        public Ent_Rol eRol { get; set; }

        public Ent_Privilegio ePrivilegio { get; set; }

        public bool Aut_Condicion { get; set; }
    }
}

using System;

namespace Model
{
    public class Ent_Privilegio
    {
        public int Pri_Id { get; set; }

        public Ent_Modulo eModulo { get; set; }

        public Ent_Operacion eOperacion { get; set; }

        public bool Pri_Condicion { get; set; }
    }
}

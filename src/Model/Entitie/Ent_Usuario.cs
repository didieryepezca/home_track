using System;

namespace Model
{
    public class Ent_Usuario
    {
        public int Usu_Id { get; set; }

        public string Usu_NumRUT { get; set; }

        public string Usu_NomApeRazSoc { get; set; }

        public string Usu_NumTelMov { get; set; }

        public string Usu_Email { get; set; }

        public string Usu_Domicilio { get; set; }

        public byte[] Usu_Salt { get; set; }

        public byte[] Usu_Pass { get; set; }

        public string Usu_RefPass { get; set; }

        public string Usu_Observacion { get; set; }

        public DateTime Usu_FecHorRegistro { get; set; }

        public bool Usu_Condicion { get; set; }

        public string Usu_Clave { get; set; }

        public Ent_Rol eRol { get; set; }

        public string Usu_TokenActualizado { get; set; }

        public DateTime Usu_FecHorTokenActualizado { get; set; }
    }
}

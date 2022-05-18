using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Dto.v1
{
    public class UsuarioNuevoDTO
    {
        public string Usu_NumRUT { get; set; }

        public string Usu_NomApeRazSoc { get; set; }

        public string Usu_NumTelMov { get; set; }

        public string Usu_Email { get; set; }

        public string Usu_Domicilio { get; set; }

        public string Usu_Observacion { get; set; }

        public string Usu_Clave { get; set; }

        public Ent_Rol eRol { get; set; }
    }
}

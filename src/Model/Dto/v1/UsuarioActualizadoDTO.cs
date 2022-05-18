using System.ComponentModel.DataAnnotations;

namespace Model.Dto.v1
{
    public class UsuarioActualizadoDTO
    {
        public string Usu_NumRUT { get; set; }

        public string Usu_NomApeRazSoc { get; set; }

        public string Usu_NumTelMov { get; set; }

        public string Usu_Email { get; set; }

        public string Usu_Domicilio { get; set; }

        public string Usu_Observacion { get; set; }

        public Ent_Rol eRol { get; set; }
    }
}

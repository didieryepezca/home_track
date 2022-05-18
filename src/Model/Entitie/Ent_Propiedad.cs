using System;

namespace Model
{
    public class Ent_Propiedad
    {
        public int Pro_Id { get; set; }

        public string Pro_Ubicacion { get; set; }

        public Ent_Tipo_Propiedad eTipo_Propiedad { get; set; }

        public Ent_Estado_Propiedad eEstado_Propiedad { get; set; }

        public decimal Pro_Valor { get; set; }

        public string Pro_Observacion { get; set; }

        public DateTime Pro_FecHorRegistro { get; set; }

        public bool Pro_Condicion { get; set; }
    }
}

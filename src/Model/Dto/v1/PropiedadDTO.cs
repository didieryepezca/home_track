namespace Model.Dto.v1;

public record PropiedadDTO(int Pro_Id, string Pro_Ubicacion, Ent_Tipo_Propiedad eTipo_Propiedad, Ent_Estado_Propiedad eEstado_Propiedad, decimal Pro_Valor, DateTime Pro_FecHorRegistro, bool Pro_Condicion);

public record PropiedadDTO_Obten_x_Id(int Pro_Id, string Pro_Ubicacion, Ent_Tipo_Propiedad eTipo_Propiedad, Ent_Estado_Propiedad eEstado_Propiedad, decimal Pro_Valor, DateTime Pro_FecHorRegistro, bool Pro_Condicion);
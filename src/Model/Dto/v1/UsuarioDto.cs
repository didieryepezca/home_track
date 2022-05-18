namespace Model.Dto.v1;

public record UsuarioDto(int Usu_Id, string Usu_NumRUT, string Usu_NomApeRazSoc, string Usu_NumTelMov, string Usu_Email, Ent_Rol eRol, DateTime Usu_FecHorRegistro, bool Usu_Condicion);

public record UsuarioDto_Obten_x_Id(int Usu_Id, string Usu_NumRUT, string Usu_NomApeRazSoc, string Usu_NumTelMov, string Usu_Email, string Usu_Domicilio, string Usu_Observacion, DateTime Usu_FecHorRegistro, bool Usu_Condicion, Ent_Rol eRol);
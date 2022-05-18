using Model;

namespace Home_Track_WebApp.HttpRepository
{
    public interface IHttpAutorizacionRepository
    {
        Task<int> Obten_Cantidad(string Rol_Nombre, string Mod_Nombre, string Ope_Nombre);
    }
}

using Model;
using Model.Dto.v1;

namespace Home_Track_WebApp.HttpRepository
{
    public interface IHttpRolRepository
    {
        Task<List<Ent_Rol>> Obten();
    }
}

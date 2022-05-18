using Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitOfWork_Interface;

namespace Service
{
    public interface IAutorizacionService
    {
        Task<int> Obten_Cantidad(string Rol_Nombre, string Mod_Nombre, string Ope_Nombre);
    }

    public class AutorizacionService : IAutorizacionService
    {
        private IUnitOfWork _unitOfWork;

        public AutorizacionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Obten_Cantidad(string Rol_Nombre, string Mod_Nombre, string Ope_Nombre)
        {
            return await Task.Run(() => {

                using var context = _unitOfWork.Create();

                return context.Repositories.AutorizacionRepository.Obten_Cantidad(Rol_Nombre, Mod_Nombre, Ope_Nombre);
            });
        }
    }
}
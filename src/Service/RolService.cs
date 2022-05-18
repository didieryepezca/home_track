using Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitOfWork_Interface;

namespace Service
{
    public interface IRolService
    {
        Task<IEnumerable<Ent_Rol>> Obten();
    }

    public class RolService : IRolService
    {
        private IUnitOfWork _unitOfWork;

        public RolService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Ent_Rol>> Obten()
        {
            return await Task.Run(() =>
            {
                using var context = _unitOfWork.Create();

                return context.Repositories.RolRepository.Obten();
            });
        }
    }
}

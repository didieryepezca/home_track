using Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitOfWork_Interface;

namespace Service
{
    public interface IEstado_PropiedadService
    {
        Task<IEnumerable<Ent_Estado_Propiedad>> Obten();
    }

    public class Estado_PropiedadService : IEstado_PropiedadService
    {
        private IUnitOfWork _unitOfWork;

        public Estado_PropiedadService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Ent_Estado_Propiedad>> Obten()
        {
            return await Task.Run(() =>
            {
                using var context = _unitOfWork.Create();

                return context.Repositories.Estado_PropiedadRepository.Obten();
            });
        }
    }
}

using Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitOfWork_Interface;

namespace Service
{
    public interface ITipo_PropiedadService
    {
        Task<IEnumerable<Ent_Tipo_Propiedad>> Obten();
    }

    public class Tipo_PropiedadService : ITipo_PropiedadService
    {
        private IUnitOfWork _unitOfWork;

        public Tipo_PropiedadService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Ent_Tipo_Propiedad>> Obten()
        {
            return await Task.Run(() =>
            {
                using var context = _unitOfWork.Create();

                return context.Repositories.Tipo_PropiedadRepository.Obten();
            });
        }
    }
}

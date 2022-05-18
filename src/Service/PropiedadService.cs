using Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitOfWork_Interface;

namespace Service
{
    public interface IPropiedadService
    {
        Task<(int, int, bool, bool, IEnumerable<Ent_Propiedad>)> Obten_Paginado(int RegistroPagina, int NumeroPagina, string PorUbicacion);

        //Task<Ent_Propiedad> Obten_x_Id(int Usu_Id);

        //Task<int> Existe(string Usu_NumRUT, bool Usu_Condicion);

        //Task<int> Crea(Ent_Propiedad Propiedad);

        //Task<int> Existente(int Usu_Id, string Usu_NumRUT, bool Usu_Condicion);

        //Task<int> Actualiza(Ent_Propiedad Propiedad);
    }

    public class PropiedadService : IPropiedadService
    {
        private IUnitOfWork _unitOfWork;

        public PropiedadService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(int, int, bool, bool, IEnumerable<Ent_Propiedad>)> Obten_Paginado(int RegistroPagina, int NumeroPagina, string PorUbicacion)
        {
            return await Task.Run(() =>
            {
                using var context = _unitOfWork.Create();

                return context.Repositories.PropiedadRepository.Obten_Paginado(RegistroPagina, NumeroPagina, PorUbicacion);
            });
        }

        //public async Task<Ent_Propiedad> Obten_x_Id(int Usu_Id)
        //{
        //    return await Task.Run(() => {

        //        using var context = _unitOfWork.Create();

        //        return context.Repositories.PropiedadRepository.Obten_x_Id(Usu_Id);
        //    });
        //}

        //public async Task<int> Existe(string Usu_NumRUT, bool Usu_Condicion)
        //{
        //    return await Task.Run(() => {

        //        using var context = _unitOfWork.Create();

        //        return context.Repositories.PropiedadRepository.Existe(Usu_NumRUT, Usu_Condicion);
        //    });
        //}

        //public async Task<int> Crea(Ent_Propiedad Propiedad)
        //{
        //    return await Task.Run(() => {

        //        using var context = _unitOfWork.Create();

        //        var Usu_Id = context.Repositories.PropiedadRepository.Crea(Propiedad);

        //        if (Usu_Id > 0)
        //        {
        //            context.SaveChanges();

        //            return Usu_Id;
        //        }
        //        else
        //        {
        //            return Usu_Id;
        //        }
        //    });
        //}

        //public async Task<int> Existente(int Usu_Id, string Usu_NumRUT, bool Usu_Condicion)
        //{
        //    return await Task.Run(() => {

        //        using var context = _unitOfWork.Create();

        //        return context.Repositories.PropiedadRepository.Existente(Usu_Id, Usu_NumRUT, Usu_Condicion);
        //    });
        //}

        //public async Task<int> Actualiza(Ent_Propiedad Propiedad)
        //{
        //    return await Task.Run(() => {

        //        using var context = _unitOfWork.Create();

        //        var CantidadAfectado = context.Repositories.PropiedadRepository.Actualiza(Propiedad);

        //        if (CantidadAfectado > 0)
        //        {
        //            context.SaveChanges();

        //            return CantidadAfectado;
        //        }
        //        else
        //        {
        //            return CantidadAfectado;
        //        }
        //    });
        //}
    }
}

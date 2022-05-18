using Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitOfWork_Interface;

namespace Service
{
    public interface IUsuarioService
    {
        Task<(int, int, bool, bool, IEnumerable<Ent_Usuario>)> Obten_Paginado(int RegistroPagina, int NumeroPagina, string PorNombre);

        Task<int> Obten_Login(string Adm_Email, string Adm_Contra);

        Task<Ent_Usuario> Obten_x_Id(int Usu_Id);

        Task<Ent_Usuario> Obten_x_Email(string Adm_Email);

        Task<bool> Actualiza_Token(Ent_Usuario Usuario);

        Task<int> Existe(string Usu_NumRUT, bool Usu_Condicion);

        Task<int> Crea(Ent_Usuario Usuario);

        Task<int> Existente(int Usu_Id, string Usu_NumRUT, bool Usu_Condicion);

        Task<int> Actualiza(Ent_Usuario Usuario);
    }

    public class UsuarioService : IUsuarioService
    {
        private IUnitOfWork _unitOfWork;

        public UsuarioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(int, int, bool, bool, IEnumerable<Ent_Usuario>)> Obten_Paginado(int RegistroPagina, int NumeroPagina, string PorNombre)
        {
            return await Task.Run(() =>
            {
                using var context = _unitOfWork.Create();

                return context.Repositories.UsuarioRepository.Obten_Paginado(RegistroPagina, NumeroPagina, PorNombre);
            });
        }

        public async Task<int> Obten_Login(string Adm_Email, string Adm_Contra)
        {
            return await Task.Run(() => {

                using var context = _unitOfWork.Create();

                return context.Repositories.UsuarioRepository.Obten_Login(Adm_Email, Adm_Contra);
            });
        }

        public async Task<Ent_Usuario> Obten_x_Id(int Usu_Id)
        {
            return await Task.Run(() => {

                using var context = _unitOfWork.Create();

                return context.Repositories.UsuarioRepository.Obten_x_Id(Usu_Id);
            });
        }

        public async Task<Ent_Usuario> Obten_x_Email(string Adm_Email)
        {
            return await Task.Run(() => {

                using var context = _unitOfWork.Create();

                return context.Repositories.UsuarioRepository.Obten_x_Email(Adm_Email);
            });
        }

        public async Task<bool> Actualiza_Token(Ent_Usuario Usuario)
        {
            return await Task.Run(() => {

                using var context = _unitOfWork.Create();

                bool Afectado;

                Afectado = context.Repositories.UsuarioRepository.Actualiza_Token(Usuario);

                if (Afectado)
                {
                    context.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            });
        }

        public async Task<int> Existe(string Usu_NumRUT, bool Usu_Condicion)
        {
            return await Task.Run(() => {

                using var context = _unitOfWork.Create();

                return context.Repositories.UsuarioRepository.Existe(Usu_NumRUT, Usu_Condicion);
            });
        }

        public async Task<int> Crea(Ent_Usuario Usuario)
        {
            return await Task.Run(() => {

                using var context = _unitOfWork.Create();

                var Usu_Id = context.Repositories.UsuarioRepository.Crea(Usuario);

                if (Usu_Id > 0)
                {
                    context.SaveChanges();

                    return Usu_Id;
                }
                else
                {
                    return Usu_Id;
                }
            });
        }

        public async Task<int> Existente(int Usu_Id, string Usu_NumRUT, bool Usu_Condicion)
        {
            return await Task.Run(() => {

                using var context = _unitOfWork.Create();

                return context.Repositories.UsuarioRepository.Existente(Usu_Id, Usu_NumRUT, Usu_Condicion);
            });
        }

        public async Task<int> Actualiza(Ent_Usuario Usuario)
        {
            return await Task.Run(() => {

                using var context = _unitOfWork.Create();

                var CantidadAfectado = context.Repositories.UsuarioRepository.Actualiza(Usuario);

                if (CantidadAfectado > 0)
                {
                    context.SaveChanges();

                    return CantidadAfectado;
                }
                else
                {
                    return CantidadAfectado;
                }
            });
        }
    }
}

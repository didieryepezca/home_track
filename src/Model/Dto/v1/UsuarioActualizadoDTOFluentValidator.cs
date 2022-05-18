using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto.v1
{
    public class UsuarioActualizadoDTOFluentValidator : AbstractValidator<UsuarioActualizadoDTO>
    {
        public UsuarioActualizadoDTOFluentValidator()
        {
            RuleFor(x => x.Usu_NumRUT)
                .NotEmpty().WithMessage("RUT requerido.")
                .Length(10).WithMessage("10 dígitos.");

            RuleFor(x => x.Usu_NomApeRazSoc)
                .NotEmpty().WithMessage("Razón Social requerida.")
                .Length(1, 50).WithMessage("Máximo 50 dígitos.");

            RuleFor(x => x.Usu_NumTelMov)
                .NotEmpty().WithMessage("Número de Teléf. Móvil requerido.")
                .Length(9).WithMessage("09 dígitos.");

            RuleFor(x => x.Usu_Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email requerido.")
                .EmailAddress().WithMessage("Email con formato inválido.");
            //.MustAsync(async (value, cancellationToken) => await IsUniqueAsync(value));

            RuleFor(x => x.Usu_Domicilio)
                .NotEmpty().WithMessage("Domicilio requerido.")
                .Length(1, 100).WithMessage("Máximo 100 dígitos.");

            RuleFor(x => x.eRol.Rol_Nombre)
                .NotEmpty().WithMessage("Rol requerido.")
                .Length(1, 50).WithMessage("Máximo 50 dígitos.");
        }

        //private async Task<bool> IsUniqueAsync(string email)
        //{
        //    // Simulates a long running http call
        //    await Task.Delay(2000);
        //    return email.ToLower() != "test@test.com";
        //}

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<UsuarioActualizadoDTO>.CreateWithOptions((UsuarioActualizadoDTO)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}

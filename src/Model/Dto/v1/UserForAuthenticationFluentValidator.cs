using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto.v1
{
    public class UserForAuthenticationFluentValidator : AbstractValidator<UserForAuthenticationDto>
    {
        public UserForAuthenticationFluentValidator()
        {
            RuleFor(x => x.Adm_Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email es requerido.")
                .EmailAddress().WithMessage("Email con formato inválido.");
            //.MustAsync(async (value, cancellationToken) => await IsUniqueAsync(value));

            RuleFor(x => x.Adm_Contra)
                .NotEmpty().WithMessage("Clave requerida.")
                .Length(1, 30).WithMessage("Máximo 30 dígitos."); ;
        }

        //private async Task<bool> IsUniqueAsync(string email)
        //{
        //    // Simulates a long running http call
        //    await Task.Delay(2000);
        //    return email.ToLower() != "test@test.com";
        //}

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<UserForAuthenticationDto>.CreateWithOptions((UserForAuthenticationDto)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}


using FluentValidation;
using ShopPhone.Shared.Request;
using System.Data;

namespace ShopPhone.Server.Validators;
public class LoginValidator :AbstractValidator<LoginRequestDTO>
{
    /// <summary>
    /// https://docs.fluentvalidation.net/en/latest/async.html
    /// </summary>
    public LoginValidator()
    {

        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(u => u.UserName).
               Cascade(CascadeMode.Stop).
               NotNull().NotEmpty().WithMessage("El usuario es un dato requerido").               
               MinimumLength(5).WithMessage("El largo mínimo es 5 caracteres");
               

        RuleFor(u => u.Password).
               Cascade(CascadeMode.Stop).
               NotNull().NotEmpty().WithMessage("La contrasenna es un dato requerido").
               MinimumLength(5).WithMessage("El largo mínimo es 5 caracteres");
    }
}

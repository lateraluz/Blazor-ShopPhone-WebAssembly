using FluentValidation;
using ShopPhone.Services.Implementations;
using ShopPhone.Services.Interfaces;
using ShopPhone.Shared.Response;

namespace ShopPhone.Server.Validators;



public class ClienteValidator : AbstractValidator<ClienteDTO>
{
    private IClienteService _clienteService;

    public ClienteValidator(IClienteService clienteService)
    {
        _clienteService = clienteService;

        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(u => u.IdCliente).
            Cascade(CascadeMode.Stop).
            NotEmpty().WithMessage("Código es requerido").
            Must(BeUnique).WithMessage("El código {PropertyValue} ya está registrado").
            GreaterThan(0).WithMessage("El código debe ser mayor que cero");

        RuleFor(u => u.Nombre).
                Cascade(CascadeMode.Stop).
                NotEmpty().WithMessage("{PropertyName} es requerido").
                MaximumLength(40).WithMessage("{PropertyName} no debe exceder 40 caracteres");

        RuleFor(u => u.Apellidos).
                Cascade(CascadeMode.Stop).
                NotEmpty().WithMessage("{PropertyName} son requeridos").
                MaximumLength(60).WithMessage("{PropertyName} no deben exceder 60 caracteres");

        RuleFor(u => u.CorreoElectronico).
                Cascade(CascadeMode.Stop).
                NotEmpty().WithMessage("{PropertyName} es requerido").
                MaximumLength(50).WithMessage("{PropertyName} no deben exceder 50 caracteres");

        RuleFor(u => u.Telefono).
               Cascade(CascadeMode.Stop).
               NotEmpty().WithMessage("{PropertyName} es requerido").
               MaximumLength(20).WithMessage("{PropertyName} no deben exceder 50 caracteres");

        RuleFor(u => u.FechaNacimiento).
              Cascade(CascadeMode.Stop).
              Must(ValidRange).WithMessage("{PropertyName} debe ser menor a hoy, el valor recibido es {PropertyValue}");

    }

    protected bool ValidRange(DateTime date)
    {
        return date <= DateTime.Now;
    }

    private bool BeUnique(int id)
    {
        return _clienteService.FindByIdAsync(id) == null;
    } 

}
using Azure;
using FluentValidation;
using ShopPhone.DataAccess;
using ShopPhone.Services.Implementations;
using ShopPhone.Services.Interfaces;
using ShopPhone.Shared.Request;
using ShopPhone.Shared.Response;

namespace ShopPhone.Server.Validators;

public class CategoriaValidator : AbstractValidator<CategoriaDTO>
{
    private ICategoriaService _categoriaService;
    private IHttpContextAccessor _context;
    public CategoriaValidator(ICategoriaService categoriaService, IHttpContextAccessor context)
    {
        _context = context;
        _categoriaService = categoriaService;

        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(u => u.IdCategoria).
                 Cascade(CascadeMode.Stop).                
                 GreaterThan(0).WithMessage("El código debe ser mayor que cero");

        // Just POST "insert" doesn't allow duplicates!
        if (_context!.HttpContext!.Request.Method.Equals("POST", StringComparison.CurrentCultureIgnoreCase)) {
            RuleFor(u => u.IdCategoria).
               Cascade(CascadeMode.Stop).
               MustAsync(async (id, cancellation) =>
               {
                   var response = await _categoriaService.FindByIdAsync(id);
                   return response.Data == null;
               }).WithMessage("El código {PropertyValue} ya esta registrado");
        }

        RuleFor(u => u.NombreCategoria).
                Cascade(CascadeMode.Stop).
                NotEmpty().WithMessage("{PropertyName} es requerida").
                MaximumLength(50).WithMessage("{PropertyName} no debe exceder 50 caracteres");
    }

  
}


using FluentValidation;
using ShopPhone.Services.Implementations;
using ShopPhone.Services.Interfaces;
using ShopPhone.Shared.Response;

namespace ShopPhone.Server.Validators;

public class ProductoValidator : AbstractValidator<ProductoDTO>
{

    private IProductoService _productoService;
    private ICategoriaService _categoriaService;
    private IHttpContextAccessor _context;
    public ProductoValidator(IProductoService productoService,  ICategoriaService categoriaService, IHttpContextAccessor context)
    {
        _context = context;
        _productoService = productoService;
        _categoriaService = categoriaService;

        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(u => u.IdProducto).
                 Cascade(CascadeMode.Stop).
                 NotEmpty().WithMessage("Código es requerido").
                 GreaterThan(0).WithMessage("El código debe ser mayor que cero");

        // Just POST "insert" doesn't allow duplicates!
        if (_context!.HttpContext!.Request.Method.Equals("POST", StringComparison.CurrentCultureIgnoreCase))
        {
            RuleFor(u => u.IdProducto).
               Cascade(CascadeMode.Stop).
               MustAsync(async (id, cancellation) =>
               {
                   var response = await _productoService.FindByIdAsync(id);
                   return response.Data == null;
               }).WithMessage("El código {PropertyValue} ya esta registrado");
        }

        RuleFor(u => u.Descripcion).
                Cascade(CascadeMode.Stop).
                NotEmpty().WithMessage("{PropertyName} es requerida").
                MaximumLength(50).WithMessage("{PropertyName} no debe exceder 100 caracteres");


        RuleFor(u => u.IdCategoria).
                 Cascade(CascadeMode.Stop).
                 NotEmpty().WithMessage("{PropertyName} es requerido").
                 GreaterThan(0).WithMessage("El código debe ser mayor que cero");


        RuleFor(u => u.IdCategoria).
              Cascade(CascadeMode.Stop).
              MustAsync(async (id, cancellation) =>
              {
                  var response = await _categoriaService.FindByIdAsync(id);
                  return !(response.Data == null);
              }).WithMessage("{PropertyName} con el valor {PropertyValue} no existe");

        RuleFor(u => u.Inventario).
                 Cascade(CascadeMode.Stop).
                 NotEmpty().WithMessage("{PropertyName} es requerido").
                 GreaterThan(0).WithMessage("{PropertyName} debe ser mayor que cero");

        RuleFor(u => u.PrecioUnitario).
                 Cascade(CascadeMode.Stop).
                 NotEmpty().WithMessage("{PropertyName} es requerido").
                 GreaterThan(0).WithMessage("{PropertyName} debe ser mayor que cero");

        RuleFor(u => u.Comentarios).
                Cascade(CascadeMode.Stop).
                MaximumLength(200).WithMessage("{PropertyName} no debe exceder a 200"); 

    }
     
}
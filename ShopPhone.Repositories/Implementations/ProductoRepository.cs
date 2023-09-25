using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopPhone.DataAccess;
using ShopPhone.Repositories.Interfaces;
using ShopPhone.Shared.Response;
using System.Reflection;



namespace ShopPhone.Repositories.Implementations;

public class ProductoRepository : IProductoRepository
{
    private ILogger<ProductoRepository> _logger;    
    private readonly ShopPhoneContext _context;
    public ProductoRepository(ShopPhoneContext context, ILogger<ProductoRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ICollection<Producto>> FindByDescriptionAsync(string description)
    {
        try
        {
            var response = await _context
                                .Set<Producto>()
                                .Include(c => c.IdCategoriaNavigation)
                                .Where(p => p.Descripcion.Contains(description))
                                .ToListAsync();
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }


    public async Task<BaseResponse> AddAsync(Producto entity)
    {
        try
        {
            entity.LastUpdate = DateTime.Now;
            await _context.Set<Producto>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return new BaseResponse() { Success = true };
        }
        catch (DbUpdateConcurrencyException concurrencyError)
        {
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", concurrencyError);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var entity = await FindAsync(id);
            if (entity != null)
            {
                entity.Estado = false;
                entity.LastUpdate = DateTime.Now;
                await UpdateAsync();
            }
            else
            {
                throw new InvalidOperationException($"No se encontro el registro con el Id {id}");
            }
        }
        catch (DbUpdateConcurrencyException concurrencyError)
        {
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", concurrencyError);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }

    public async Task<Producto?> FindAsync(int id)
    {
        try
        {
            var response = await _context
                                .Set<Producto>()
                                .FindAsync(id);
            return response!;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }

    public async Task<BaseResponse> UpdateAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            return new BaseResponse() { Success = true };
        }
        catch (DbUpdateConcurrencyException concurrencyError)
        {
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", concurrencyError);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }

    }

    public async Task<ICollection<Producto>> ListAsync()
    {
        try
        { 
            var response = await _context
                                .Set<Producto>()
                                .Include(c=>c.IdCategoriaNavigation)
                                .ToListAsync();
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }
}

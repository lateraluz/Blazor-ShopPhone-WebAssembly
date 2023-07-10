using Azure;
using log4net;
using Microsoft.EntityFrameworkCore;
using ShopPhone.DataAccess;
using ShopPhone.Shared.Response;
using System.Reflection;
using System.Security.Principal;


namespace ShopPhone.Repositories.Implementations;

public class ProductoRepository : IProductoRepository
{
    private ILog _Logger;
    private readonly ShopphoneContext _Context;
    public ProductoRepository(ShopphoneContext context, ILog logger)
    {
        _Context = context;
        _Logger = logger;
    }

    public async Task<ICollection<Producto>> FindByDescriptionAsync(string description)
    {
        try
        {
            var response = await _Context
                                .Set<Producto>()
                                .Where(p => p.Descripcion.Contains(description))
                                .ToListAsync();
            return response;
        }
        catch (Exception ex)
        {
            _Logger.Error(ex.Message);
            throw;
        }
    }


    public async Task<BaseResponse> AddAsync(Producto entity)
    {
        try
        {
            await _Context.Set<Producto>().AddAsync(entity);
            await _Context.SaveChangesAsync();
            return new BaseResponse() { Success = true };
        }
        catch (Exception ex)
        {
            _Logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);            
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
                await UpdateAsync();
            }
            else
            {
                throw new InvalidOperationException($"No se encontro el registro con el Id {id}");
            }
        }
        catch (Exception ex)
        {
            _Logger.Error(ex.Message);
            throw;
        }
    }

    public async Task<Producto?> FindAsync(int id)
    {
        try
        {
            var response = await _Context
                                .Set<Producto>()
                                .FindAsync(id);
            return response!;
        }
        catch (Exception ex)
        {
            _Logger.Error(ex.Message);
            throw;
        }
    }

    public async Task<BaseResponse> UpdateAsync()
    {
        try
        {
            await _Context.SaveChangesAsync();
            return new BaseResponse() { Success = true };
        }
        catch (Exception ex)
        {
            _Logger.Error(ex.Message);
            throw;
        }

    }

    public async Task<ICollection<Producto>> ListAsync()
    {
        try
        { 
            var response = await _Context
                                .Set<Producto>()
                                .Include(c=>c.IdCategoriaNavigation)
                                .ToListAsync();
            return response;
        }
        catch (Exception ex)
        {
            _Logger.Error(ex.Message);
            throw;
        }
    }
}

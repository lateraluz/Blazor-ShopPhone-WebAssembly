using Microsoft.EntityFrameworkCore;
using ShopPhone.DataAccess;
using ShopPhone.Repositories.Interfaces;
using System.Reflection;
namespace ShopPhone.Repositories.Implementations;

using Azure.Core;
using Microsoft.Extensions.Logging;


public class CategoriaRepository : ICategoriaRepository
{

    private ILogger<CategoriaRepository> _logger;
    private readonly ShopPhoneContext _context;                     
    public CategoriaRepository(ShopPhoneContext context, ILogger<CategoriaRepository> logger)
    {
        _context = context;
        _logger = logger;
    }


    public async Task<ICollection<Categorium>> FindByDescriptionAsync(string description)
    {
        try
        {          

            var response = await _context
                                .Set<Categorium>()
                                .AsNoTracking()
                                .Where(p => p.NombreCategoria.Contains(description))
                                .ToListAsync();
            return response;

        }
        catch (Exception ex)
        {
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }

    }

    public async Task<ICollection<Categorium>> ListAsync()
    {
        try
        {

            _logger.LogInformation($"List all Categoria");
            var response = await _context
                                .Set<Categorium>()
                                .AsNoTracking()                               
                                .ToListAsync();
            return response;

        }
        catch (Exception ex)
        {
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }

    }


    public async Task<int> AddAsync(Categorium entity)
    {
        try
        {
            entity.LastUpdate = DateTime.Now;
            await _context.Database.BeginTransactionAsync();
            await _context.Set<Categorium>().AddAsync(entity);
            await _context.SaveChangesAsync();
            await _context.Database.CommitTransactionAsync(); ;
            return entity.IdCategoria;
        }
        catch (DbUpdateConcurrencyException concurrencyError) {
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
                throw new Exception($"No se encontro el registro con el Id {id}");
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

    public async Task<Categorium?> FindAsync(int id)
    {
        try
        {
            var response = await _context
                                .Set<Categorium>()
                                .FindAsync(id);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        } 
    } 

    public async Task UpdateAsync()
    {
        try
        {            
            await _context.SaveChangesAsync();
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
 
     
}
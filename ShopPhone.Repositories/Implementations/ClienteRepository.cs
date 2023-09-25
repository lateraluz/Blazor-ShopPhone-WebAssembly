using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopPhone.DataAccess;
using ShopPhone.Repositories.Interfaces;
using System.Reflection;

namespace ShopPhone.Repositories.Implementations;


public class ClienteRepository : IClienteRepository
{

    private ILogger<ClienteRepository> _logger;
    private readonly ShopPhoneContext _context;
    public ClienteRepository(ShopPhoneContext context, ILogger<ClienteRepository> logger)
    {
        _context = context;
        _logger = logger;
    }


    public async Task<ICollection<Cliente>> FindByDescriptionAsync(string description)
    {
        try
        {
            var response = await _context
                                .Set<Cliente>()
                                .Where(p => p.Nombre.Contains(description) || p.Apellidos.Contains(description))
                                .ToListAsync();
            return response;

        }
        catch (Exception ex)
        {
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }

    }

    public async Task<ICollection<Cliente>> ListAsync()
    {
        try
        {         

            var response = await _context
                                .Set<Cliente>()
                                .ToListAsync();
            return response;

        }
        catch (Exception ex)
        {
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }

    }


    public async Task<int> AddAsync(Cliente entity)
    {
        try
        {
            entity.LastUpdate = DateTime.Now;
            await _context.Set<Cliente>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdCliente;
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
                entity.LastUpdate = DateTime.Now;
                entity.Estado = false;
                await UpdateAsync();
            }
            else
            {
                throw new InvalidOperationException($"No se encontro el registro con el Id {id}");
            }
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

    public async Task<Cliente?> FindAsync(int id)
    {
        try
        {
            var response = await _context
                                .Set<Cliente>()
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

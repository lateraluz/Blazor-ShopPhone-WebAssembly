using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopPhone.DataAccess;
using ShopPhone.Repositories.Interfaces;
using System.Reflection;

namespace ShopPhone.Repositories.Implementations;


public class ClienteRepository : IClienteRepository
{

    private ILogger<ClienteRepository> _logger;
    private readonly ShopPhoneContext _Context;
    public ClienteRepository(ShopPhoneContext context, ILogger<ClienteRepository> logger)
    {
        _Context = context;
        _logger = logger;
    }


    public async Task<ICollection<Cliente>> FindByDescriptionAsync(string description)
    {
        try
        {
            var response = await _Context
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
            var response = await _Context
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
            await _Context.Set<Cliente>().AddAsync(entity);
            await _Context.SaveChangesAsync();
            return entity.IdCliente;
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
                await UpdateAsync();
            }
            else
            {
                throw new InvalidOperationException($"No se encontro el registro con el Id {id}");
            }
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
            var response = await _Context
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
            await _Context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }

    }


}

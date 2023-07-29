using log4net;
using Microsoft.EntityFrameworkCore;
using ShopPhone.DataAccess;
using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Repositories.Implementations;


public class ClienteRepository : IClienteRepository
{

    private ILog _Logger;
    private readonly ShopPhoneContext _Context;
    public ClienteRepository(ShopPhoneContext context, ILog logger)
    {
        _Context = context;
        _Logger = logger;
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
        catch (Exception e)
        {
            _Logger.Error(e.Message);
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
        catch (Exception e)
        {
            _Logger.Error(e.Message);
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
        catch (Exception e)
        {
            _Logger.Error(e.Message);
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
        catch (Exception e)
        {
            _Logger.Error(e.Message);
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
        catch (Exception e)
        {
            _Logger.Error(e.Message);
            throw;
        }
    }

    public async Task UpdateAsync()
    {
        try
        {
            await _Context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _Logger.Error(e.Message);
            throw;
        }

    }


}

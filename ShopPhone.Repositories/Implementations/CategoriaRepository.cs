using log4net;
using Microsoft.EntityFrameworkCore;
using ShopPhone.DataAccess;
using ShopPhone.Repositories.Interfaces;

namespace ShopPhone.Repositories.Implementations;


public class CategoriaRepository : ICategoriaRepository
{

    private ILog _Logger;
    private readonly ShopPhoneContext _Context;                     
    public CategoriaRepository(ShopPhoneContext context, ILog logger)
    {
        _Context = context;
        _Logger = logger;
    }


    public async Task<ICollection<Categorium>> FindByDescriptionAsync(string description)
    {
        try
        {
            var response = await _Context
                                .Set<Categorium>()
                                .Where(p => p.NombreCategoria.Contains(description))
                                .ToListAsync();
            return response;

        }
        catch (Exception e)
        {
            _Logger.Error(e.Message);
            throw;
        }

    }

    public async Task<ICollection<Categorium>> ListAsync()
    {
        try
        {
            var response = await _Context
                                .Set<Categorium>()                               
                                .ToListAsync();
            return response;

        }
        catch (Exception e)
        {
            _Logger.Error(e.Message);
            throw;
        }

    }


    public async Task<int> AddAsync(Categorium entity)
    {
        try
        {
            await _Context.Database.BeginTransactionAsync();
            await _Context.Set<Categorium>().AddAsync(entity);
            await _Context.SaveChangesAsync();
            await _Context.Database.CommitTransactionAsync(); ;
            return entity.IdCategoria;
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

    public async Task<Categorium?> FindAsync(int id)
    {
        try
        {
            var response = await _Context
                                .Set<Categorium>()
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
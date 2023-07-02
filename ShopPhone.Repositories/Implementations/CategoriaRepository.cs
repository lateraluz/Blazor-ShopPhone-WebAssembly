using Azure;
using log4net;
using Microsoft.EntityFrameworkCore;
using ShopPhone.DataAccess;
using ShopPhone.Shared.Entities;
using ShopPhone.Shared.Response;
using ShopPhone.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Repositories.Implementations;


public class CategoriaRepository : ICategoriaRepository
{

    private ILog _Logger;
    private readonly ShopphoneContext _Context;
    public CategoriaRepository(ShopphoneContext context, ILog logger)
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
     

    public async Task<int> AddAsync(Categorium entity)
    {
        try
        {
            await _Context.Set<Categorium>().AddAsync(entity);
            await _Context.SaveChangesAsync();
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
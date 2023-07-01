using log4net;
using Microsoft.EntityFrameworkCore;
using ShopPhone.DataAccess;
using ShopPhone.Shared.Entities;
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

        var response = await _Context.Set<Categorium>().Where(p => p.NombreCategoria.Contains(description)).ToListAsync();

        return response;

    }



}
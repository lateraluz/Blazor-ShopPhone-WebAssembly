using log4net.Core;
using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Services.Implementations;

public class CategoriaService : ICategoriaService
{
    /*private ILogger<CategoriaService> _logger;

    public CategoriaService(ILogger<CategoriaService> logger ) {
        _logger = logger;
    }
    */
    public async Task<BaseResponseGeneric<ICollection<CategoriaDTO>>> FindByDescriptionAsync(string description)
    {

        try
        {
            //_LogTrace.Info($"consulta {Nombre} {Apellido}");

            List<CategoriaDTO> lista = new List<CategoriaDTO>();
            lista.Add(new CategoriaDTO() { IdCategoria = 1, NombreCategoria = "Telefono 4G", Estado = true });
            lista.Add(new CategoriaDTO() { IdCategoria = 2, NombreCategoria = "Telefono 5G", Estado = true });
            lista.Add(new CategoriaDTO() { IdCategoria = 3, NombreCategoria = "Tablet 3G", Estado = true });
            lista.Add(new CategoriaDTO() { IdCategoria = 4, NombreCategoria = "Tablet 3G", Estado = true });

            BaseResponseGeneric<ICollection<CategoriaDTO>> response = new BaseResponseGeneric<ICollection<CategoriaDTO>>();

            response.Success = true;
            response.Data = lista;

            return await Task.FromResult(response);
        }
        catch (Exception e)
        {
            //_LogEvents.Error(e.Message);
            throw;
        }
    }

}

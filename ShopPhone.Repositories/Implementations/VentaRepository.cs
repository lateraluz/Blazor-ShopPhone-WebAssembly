using log4net;
using System.Data.SqlClient;
using ShopPhone.DataAccess;
using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopPhone.Repositories.Interfaces;

namespace ShopPhone.Repositories.Implementations;

public class VentaRepository : IVentaRepository
{
    private ILog _Logger;
    private readonly ShopPhoneContext _Context;

    public VentaRepository(ShopPhoneContext context, ILog logger)
    {
        _Context = context;
        _Logger = logger;
    }

    /// <summary>
    /// Extraer un consecutivo para asignar el numero de factura
    ///  Investigue como crear secuencias en SQLServer
    /// http://technet.microsoft.com/es-es/library/ff878091.aspx
    /// CREATE SEQUENCE  NoFactura  START WITH 1 INCREMENT BY 1 ;
    /// </summary>
    /// <returns>Num. de factura</returns>
    public int GetNoReceipt()
    {
        int siguiente = 0;

        // it doesnt work
        // siguiente = _Context.Database..SqlQuery<Int32>($"SELECT NEXT VALUE FOR NumeroFactura").FirstOrDefault();
        // siguiente = _Context.Database.SqlQuery($"SELECT NEXT VALUE FOR NumeroFactura");
        try
        {
            string sql = string.Format("SELECT NEXT VALUE FOR NumeroFactura");

            System.Data.DataTable dataTable = new System.Data.DataTable();

            System.Data.Common.DbConnection connection = _Context.Database.GetDbConnection();
            System.Data.Common.DbProviderFactory dbFactory = System.Data.Common.DbProviderFactories.GetFactory(connection!)!;
            using (var cmd = dbFactory!.CreateCommand())
            {
                cmd!.Connection = connection;
                cmd.CommandText = sql;
                using (System.Data.Common.DbDataAdapter adapter = dbFactory.CreateDataAdapter()!)
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dataTable);
                }
            }

            siguiente = (int)dataTable.Rows[0][0];
            return siguiente;
        }
        catch (Exception ex)
        {
            _Logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }


    }

    public async Task<BaseResponse> AddAsync(FacturaEncabezado entity)
    {
        try
        {
            entity.FechaVenta = DateTime.Now;
            // inicio de transaccion
            await _Context.Database.BeginTransactionAsync();
            // Salvar encabezado
            await _Context.Set<FacturaEncabezado>().AddAsync(entity);
            await _Context.SaveChangesAsync();

            // Rebajar inventario
            foreach (var item in entity.FacturaDetalles)
            {
                await _Context.Database.ExecuteSqlAsync($"Update Producto set Inventario = Inventario - {item.Cantidad} where IdProducto = {item.IdProducto}");
            }
            await _Context.Database.CommitTransactionAsync();
            return new BaseResponse() { Success = true };
        }
        catch (Exception ex)
        {
            await _Context.Database.RollbackTransactionAsync();
            _Logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<FacturaEncabezado?> FindAsync(int id)
    {
        var response = await _Context
                                .Set<FacturaEncabezado>()                                
                                .Include(c => c.IdClienteNavigation)
                                .Include(g => g.FacturaDetalles)
                                .Include("FacturaDetalles.IdProductoNavigation")
                                .FirstOrDefaultAsync(p => p.IdFactura == id);
        return response;
    }

    public Task<BaseResponse> UpdateAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<FacturaEncabezado>> ListAsync()
    {
        try
        {
            var response = await _Context
                                .Set<FacturaEncabezado>()
                                .Include(c => c.IdClienteNavigation)
                                .Include(g => g.FacturaDetalles)
                                .ToListAsync();


            return response;

        }
        catch (Exception e)
        {
            _Logger.Error(e.Message);
            throw;
        }
    }
}

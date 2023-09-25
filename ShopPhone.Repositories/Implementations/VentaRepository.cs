using ShopPhone.DataAccess;
using ShopPhone.Shared.Response;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ShopPhone.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace ShopPhone.Repositories.Implementations;

public class VentaRepository : IVentaRepository
{
    private ILogger<VentaRepository> _logger;
    private readonly ShopPhoneContext _context;

    public VentaRepository(ShopPhoneContext context, ILogger<VentaRepository> logger)
    {
        _context = context;
        _logger = logger;
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

            System.Data.Common.DbConnection connection = _context.Database.GetDbConnection();
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
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }


    }

    public async Task<BaseResponse> AddAsync(FacturaEncabezado entity)
    {
        try
        {
            entity.FechaVenta = DateTime.Now;
            // inicio de transaccion
            await _context.Database.BeginTransactionAsync();
            // Salvar encabezado
            await _context.Set<FacturaEncabezado>().AddAsync(entity);
            await _context.SaveChangesAsync();

            // Rebajar inventario
            foreach (var item in entity.FacturaDetalles)
            {
                await _context.Database.ExecuteSqlAsync($"Update Producto set Inventario = Inventario - {item.Cantidad} where IdProducto = {item.IdProducto}");
            }
            await _context.Database.CommitTransactionAsync();
            return new BaseResponse() { Success = true };
        }
        catch (DbUpdateConcurrencyException concurrencyError)
        {
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", concurrencyError);
            throw;
        }
        catch (Exception ex)
        {
            await _context.Database.RollbackTransactionAsync();
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<FacturaEncabezado?> FindAsync(int id)
    {
        try
        {
            var response = await _context
                               .Set<FacturaEncabezado>()
                               .Include(c => c.IdClienteNavigation)
                               .Include(g => g.FacturaDetalles)
                               .Include("FacturaDetalles.IdProductoNavigation")
                               .FirstOrDefaultAsync(p => p.IdFactura == id);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
       
    }

    public Task<BaseResponse> UpdateAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<FacturaEncabezado>> ListAsync()
    {
        try
        {
            var response = await _context
                                .Set<FacturaEncabezado>()
                                .Include(c => c.IdClienteNavigation)
                                .Include(g => g.FacturaDetalles)
                                .ToListAsync();


            return response;

        }
        catch (Exception ex)
        {
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }
}

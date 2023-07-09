using ShopPhone.DataAccess;
using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Repositories.Implementations;

public interface IVentaRepository
{
    int GetNoReceipt();

    Task<BaseResponse> AddAsync(FacturaEncabezado entity);
    Task DeleteAsync(int id);

    Task<FacturaEncabezado?> FindAsync(int id);

    Task<BaseResponse> UpdateAsync();

    Task<ICollection<FacturaEncabezado>> ListAsync();
}

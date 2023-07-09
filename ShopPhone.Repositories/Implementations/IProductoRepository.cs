using ShopPhone.DataAccess;
using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Repositories.Implementations;

public interface IProductoRepository
{
    Task<ICollection<Producto>> FindByDescriptionAsync(string description);

    Task<BaseResponse> AddAsync(Producto entity);
    Task DeleteAsync(int id);

    Task<Producto?> FindAsync(int id);

    Task<BaseResponse> UpdateAsync();

    Task<ICollection<Producto>> ListAsync();
}

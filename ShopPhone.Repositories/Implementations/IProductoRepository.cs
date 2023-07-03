using ShopPhone.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Repositories.Implementations;

public interface IProductoRepository
{
    Task<ICollection<Producto>> FindByDescriptionAsync(string description);

    Task<int> AddAsync(Producto entity);
    Task DeleteAsync(int id);

    Task<Producto?> FindAsync(int id);

    Task UpdateAsync();
}

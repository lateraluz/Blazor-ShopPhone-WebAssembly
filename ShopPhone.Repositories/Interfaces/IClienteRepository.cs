using ShopPhone.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Repositories.Interfaces
{
    public interface IClienteRepository
    {
        Task<ICollection<Cliente>> FindByDescriptionAsync(string description);
        Task<ICollection<Cliente>> ListAsync();
        Task<int> AddAsync(Cliente entity);
        Task DeleteAsync(int id);
        Task<Cliente?> FindAsync(int id);
        Task UpdateAsync();
    }
}

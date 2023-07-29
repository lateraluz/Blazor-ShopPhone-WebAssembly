using ShopPhone.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Repositories.Implementations;

public interface IUserRepository
{
    Task<ICollection<User>> FindByNameAsync(string name);
    Task<ICollection<User>> ListAsync();
    Task<string> AddAsync(User entity);
    Task DeleteAsync(string id);
    Task<User?> FindAsync(string id);
    Task UpdateAsync();
}
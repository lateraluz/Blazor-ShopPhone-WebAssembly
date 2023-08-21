using ShopPhone.DataAccess;
using ShopPhone.Shared.Entities;
using ShopPhone.Shared.Response;

namespace ShopPhone.Repositories.Interfaces;

public interface ICategoriaRepository
{
    Task<ICollection<Categorium>> FindByDescriptionAsync(string description);
    Task<ICollection<Categorium>> ListAsync();
    Task<int> AddAsync(Categorium entity);
    Task DeleteAsync(int id);
    Task<Categorium?> FindAsync(int id);
    Task UpdateAsync();
}
using ShopPhone.DataAccess;
using ShopPhone.Shared.Entities;
using ShopPhone.Shared.Response;

namespace ShopPhone.Repositories.Implementations;

public interface ICategoriaRepository
{
    Task<ICollection<Categorium>> FindByDescriptionAsync(string description);

    Task<int> AddAsync(Categorium entity);
    Task DeleteAsync(int id);

    Task<Categorium?> FindAsync(int id);

    Task UpdateAsync();
}
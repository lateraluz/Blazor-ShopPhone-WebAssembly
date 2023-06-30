using ShopPhone.DataAccess;
using ShopPhone.Shared.Entities;

namespace ShopPhone.Repositories.Implementations
{
    public interface ICategoriaRepository
    {
        Task<ICollection<Categorium>> FindByDescriptionAsync(string description);
    }
}
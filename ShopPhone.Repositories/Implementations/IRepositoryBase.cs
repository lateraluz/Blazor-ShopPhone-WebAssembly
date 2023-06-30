using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Repositories.Implementations;

public interface IRepositoryBase<TEntity> where TEntity : EntityBase
{
    // Lista de objetos basados en el Entity
    Task<ICollection<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate);

    // Lista de objetos transformados en objeto Info (combinacion de mas de una tabla) y que
    // ademas contenga un selector
    Task<(ICollection<TInfo> Collection, int Total)> ListAsync<TInfo, TKey>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TInfo>> selector,
        Expression<Func<TEntity, TKey>> orderBy,
        int page, int rows);

    // Listar los objetos con un selector
    Task<ICollection<TInfo>> ListAsync<TInfo>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TInfo>> selector);

    Task<int> AddAsync(TEntity entity);

    Task<TEntity?> FindAsync(int id);

    Task UpdateAsync();

    Task DeleteAsync(int id);
}

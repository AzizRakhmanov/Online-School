using Domain.Commons;
using System.Linq.Expressions;

namespace DAL.IRepository
{
    public interface IRepository<TEntity> where TEntity : Auditable
    {
        ValueTask<TEntity> InsertAsync(TEntity entity);
        TEntity Update(TEntity entity);
        IQueryable<TEntity> SelectAll(Expression<Func<TEntity, bool>> expression, string[] includes);
        ValueTask<TEntity> SelectAsync(Expression<Func<TEntity, bool>> expression = null, string[] includes = null);
        ValueTask<bool> DeleteAsync(Expression<Func<TEntity, bool>> expression);
        bool DeleteMany(Expression<Func<TEntity, bool>> expression);
    }
}

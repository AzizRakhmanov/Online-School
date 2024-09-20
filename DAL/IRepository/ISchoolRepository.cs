using Domain.Commons;
using System.Linq.Expressions;

namespace DAL.IRepository
{

    public interface ISchoolRepository<Entity> where Entity : Auditable
    {
        public ValueTask<Entity> SelectAsync(Guid id);

        public Task<IEnumerable<Entity>> SelectAllAsync(Expression<Func<Entity, bool>> expression);

        public ValueTask<Entity> CreateAsync(Entity user);

        public Task Delete(Guid id);

        public Task Update(Entity user);

        public Task SaveAsync();
    }
}

using DAL.DataAccess;
using DAL.IRepository;
using Domain.Commons;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repository
{
    public class SchoolRepository<Entity> : ISchoolRepository<Entity> where Entity : Auditable
    {
        private readonly SchoolDb _schoolDb;
        private readonly DbSet<Entity> _dbSet;

        public SchoolRepository(SchoolDb db)
        {
            this._schoolDb = db;
            this._dbSet = db.Set<Entity>();
        }
        public async ValueTask<Entity> CreateAsync(Entity entity)
        {
            await this._dbSet.AddAsync(entity);
            return entity;
        }

        public async void Delete(Guid id)
        {
            var dbEntity = await this.SelectAsync(id);
            this._dbSet.Remove(dbEntity);
        }

        public async Task<IEnumerable<Entity>> SelectAllAsync(Expression<Func<Entity, bool>> expression)
        {
            var allEntities = await this._dbSet.Where(expression).ToListAsync();
            return allEntities;
        }

        public async ValueTask<Entity> SelectAsync(Guid id)
        {
            var dbEntity = await this._dbSet.FindAsync(id);
            return dbEntity;
        }

        public void Update(Entity user)
        {
            this._dbSet.Update(user);
        }

        public async Task SaveAsync()
        {
            await this._schoolDb.SaveChangesAsync();
        }
    }
}

using DAL.DataAccess;
using DAL.IRepository;
using Domain.Commons;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace DAL.Repository
{
    public partial class SchoolRepository<Entity> : ISchoolRepository<Entity> where Entity : Auditable
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
            await this._schoolDb.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var dbEntity = await this._dbSet.FindAsync(id);

            this._dbSet.Remove(dbEntity);
            return await this._schoolDb.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<IEnumerable<Entity>> SelectAllAsync(Expression<Func<Entity, bool>> expression)
        {
            var allEntities = await this._dbSet.Where(expression).ToListAsync();

            return allEntities;
        }

        public async ValueTask<Entity> SelectAsync(Guid id)
        {
            var dbEntity = await this._dbSet.FirstOrDefaultAsync(p => p.Id == id);
            return dbEntity;
        }

        public async Task<bool> UpdateAsync(Entity entity)
        {
            var dbUser = await this.SelectAsync(entity.Id);

            EntityEntry<Entity> entityEntry = this._schoolDb.Update(entity);
            return await this._schoolDb.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task SaveAsync()
        {
            await this._schoolDb.SaveChangesAsync();
        }

        public async ValueTask<bool> ExistsAsync(Expression<Func<Entity, bool>> expression)
        {
            return await this._dbSet.AsNoTracking().AnyAsync(expression);
        }
    }
}

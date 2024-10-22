using DAL.DataAccess;
using DAL.IRepository;
using Domain.Commons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace DAL.Repository
{
    public class Repository<Entity> : IRepository<Entity> where Entity : Auditable
    {
        private readonly SchoolDb _dbContext;
        private readonly DbSet<Entity> dbSet;
        public Repository(SchoolDb dbContext)
        {
            this.dbSet = dbContext.Set<Entity>(); // this._dbContext.Set<Entity>();
            this._dbContext = dbContext;
        }

        /// <summary>
        /// Deletes first item that matched expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>true if action is successful, false if unable to delete</returns>
        public async ValueTask<bool> DeleteAsync(Expression<Func<Entity, bool>> expression)
        {
            var entity = await this.SelectAsync(expression);

            this.dbSet.Entry(entity).State = EntityState.Deleted;
            await this._dbContext.SaveChangesAsync();

            return false;
        }

        /// <summary>
        /// Deletes all elements if expression matches
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool DeleteMany(Expression<Func<Entity, bool>> expression)
        {
            var entities = dbSet.Where(expression);
            if (entities.Any())
            {
                foreach (var entity in entities)
                    this.dbSet.Entry(entity).State = EntityState.Deleted;

                this._dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Inserts element to a table and keep track of it until change saved
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async ValueTask<Entity> InsertAsync(Entity entity)
        {
            EntityEntry<Entity> entry = await this.dbSet.AddAsync(entity);
            await this._dbContext.SaveChangesAsync();

            return entry.Entity;
        }

        public async ValueTask<bool> InsertAsync(IEnumerable<Entity> entity)
        {
            await this.dbSet.AddRangeAsync(entity);
            await this._dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Saves tracking changes and write them to database permenantly
        /// </summary>
        /// <returns></returns>
        //public async ValueTask SaveAsync()
        //{
        //    await dbContext.SaveChangesAsync();
        //}

        /// <summary>
        /// Selects all elements from table that matches condition and include relations
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entity> SelectAll(Expression<Func<Entity, bool>> expression, string[] includes)
        {
            IQueryable<Entity> query = expression is null ? this.dbSet : this.dbSet.Where(expression);

            if (includes is not null)
            {
                foreach (string include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query;
        }

        /// <summary>
        /// selects element from a table specified with expression and can includes relations
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async ValueTask<Entity> SelectAsync(Expression<Func<Entity, bool>> expression, string[] includes = null)
            => await this.SelectAll(expression, includes).FirstOrDefaultAsync(t => t != null);

        /// <summary>
        /// Updates entity and keep track of it until change saved
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Entity Update(Entity entity)
        {
            EntityEntry<Entity> entryentity = this._dbContext.Update(entity);
            this._dbContext.SaveChanges();

            return entryentity.Entity;
        }
    }
}


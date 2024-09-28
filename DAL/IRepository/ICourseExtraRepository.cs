using Domain.Commons;
using Domain.Models;
using System.Linq.Expressions;

namespace DAL.IRepository
{
    public interface ICourseExtraRepository
    {
        public Task<Course> SelectAsync(Guid id);
        public Task<IEnumerable<Course>> SelectAllAsync(Expression<Func<Course, bool>> expression);
    }
}

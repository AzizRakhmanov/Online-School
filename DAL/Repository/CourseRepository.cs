using DAL.DataAccess;
using DAL.IRepository;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repository
{
    public class CourseRepository : ICourseExtraRepository
    {
        private readonly SchoolDb _schoolDb;

        public CourseRepository(SchoolDb schoolDb)
        {
            this._schoolDb = schoolDb;
        }

        public async Task<IEnumerable<Course>> SelectAllAsync(Expression<Func<Course, bool>> expression)
        {
            var allCoursesWithTeachers = await this._schoolDb.Courses.Include(p => p.Teacher).ToListAsync();

            return allCoursesWithTeachers;
        }
        public async Task<Course> SelectAsync(Guid id)
        {
            var courseWithItsTeacher = await  this._schoolDb.Courses.AsNoTracking().Include(p => p.Teacher).FirstOrDefaultAsync(p => p.Id == id);

            return courseWithItsTeacher;
        }
    }
}

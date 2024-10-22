using DAL.IRepository;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Service.Services.CourseService
{
    public class CourseService : ICourseService
    {
        private readonly IRepository<Course> _repository;
        private readonly UserManager<IdentityUser>  _userManager;

        public CourseService(IRepository<Course> repository,
            UserManager<IdentityUser> userManager)
        {
            this._repository = repository;
            this._userManager = userManager;
        }

        public IEnumerable<Course> GetAll()
        {
            var includes = new string[] { "Teacher" };
            var allCourses = this._repository.SelectAll(p => p.Id != Guid.Empty, includes);
            return allCourses;
        }

        public async ValueTask<Course> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new Exception(message: "Id can't be null");

            var dbCourse = await this._repository.SelectAsync(p => p.Id == id);

            if (dbCourse is null)
                return null;

            return dbCourse;
        }

        public async ValueTask<Course> AddAsync(Course course)
        {
            var existingTeacher = await this._userManager.FindByIdAsync(course.TeacherId);

            if (existingTeacher == null)
                throw new Exception(message: "Teacher with this email address does not exist");

            var dbResponse = await this._repository.InsertAsync(course);

            return dbResponse;
        }

        public async Task<bool> UpdateAsync(Course course)
        {
            if (course is null)
                return false;

            var entity = await this._repository.SelectAsync(p => p.Id == course.Id);

            if (entity is null)
                return false;

            return this._repository.Update(course) != null ? false : true;
        }

        public async ValueTask<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                return false;

            return await this._repository.DeleteAsync(p => p.Id == id);
        }
    }
}

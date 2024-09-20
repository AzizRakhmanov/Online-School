using DAL.IRepository;
using Domain.Models;

namespace Service.Services.CourseService
{
    public class CourseService : ICourseService
    {
        private readonly ISchoolRepository<Course> _repository;

        public CourseService(ISchoolRepository<Course> repository)
        {
            this._repository = repository;
        }


        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            var allCourses = await this._repository.SelectAllAsync(p => p != null);
            return allCourses;
        }

        public async ValueTask<Course> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new Exception(message: "Id can't be null");

            var dbCourse = await this._repository.SelectAsync(id);

            if (dbCourse is null)
                return null;

            return dbCourse;
        }

        public async ValueTask<Course> AddAsync(Course course)
        {
            if (course.Id == Guid.Empty)
                course.Id = Guid.NewGuid();

            var dbResponse = await this._repository.CreateAsync(course);

            return dbResponse;
        }

        public void Update(Course course)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<bool> DeleteAsync(Guid courseId)
        {
            if (courseId == Guid.Empty)
                return false;

            await this._repository.DeleteAsync(courseId);

            var deleted = await this._repository.ExistsAsync(p => p.Id == courseId);

            if (deleted) return false;

            return true;
        }

        public async Task<bool> UserOwnsCourseAsync(Guid courseId, string userId)
        {
            var course = await this._repository.SelectAsync(courseId);

            if (course == null)
                return false;

            if (course.TeacherId != userId)
                return false;

            return true;

        }
    }
}

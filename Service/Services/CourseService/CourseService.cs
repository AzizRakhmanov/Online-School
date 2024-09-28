using DAL.IRepository;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Service.Services.CourseService
{
    public class CourseService : ICourseService
    {
        private readonly ISchoolRepository<Course> _repository;
        private readonly ICourseExtraRepository _courseExtraRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public CourseService(ISchoolRepository<Course> repository,
            ICourseExtraRepository courseExtraRepository,
            UserManager<IdentityUser> userManager)
        {
            this._repository = repository;
            this._courseExtraRepository = courseExtraRepository;
            this._userManager = userManager;
        }


        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            var allCourses = await this._courseExtraRepository.SelectAllAsync(p => p != null);
            return allCourses;
        }

        public async ValueTask<Course> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new Exception(message: "Id can't be null");

            var dbCourse = await  this._courseExtraRepository.SelectAsync(id);

            if (dbCourse is null)
                return null;

            return dbCourse;
        }

        public async ValueTask<Course> AddAsync(Course course)
        {
            var existingTeacher = await this._userManager.FindByIdAsync(course.TeacherId);

            if (existingTeacher is null)
                throw new Exception(message: "Teacher with this email address does not exist");

            course.TeacherId = existingTeacher.Id;
            var dbResponse = await this._repository.CreateAsync(course);

            return dbResponse;
        }

        public async Task<bool> UpdateAsync(Course course)
        {
            if (course is null)
                return false;

            var entityExists = await this._repository.ExistsAsync(p => p.Id == course.Id);

            if (!entityExists)
                return false;

            return await this._repository.UpdateAsync(course);
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

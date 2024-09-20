using Domain.Models;

namespace Service.Services.CourseService
{
    public interface ICourseService
    {
        public Task<IEnumerable<Course>> GetAllAsync();

        public ValueTask<Course> GetAsync(Guid id);

        public ValueTask<Course> AddAsync(Course course);

        public void Update(Course course);

        public ValueTask<bool> DeleteAsync(Guid id);

        public Task<bool> UserOwnsCourseAsync(Guid courseId, string getUserId);
    }
}

using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using OnlineSchoolCrm.ViewModels;
using OnlineSchoolCrm.ViewModels.CourseModel;
using Service.Options;
using Service.Services.CourseService;


namespace OnlineSchoolCrm.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
   // [Route("api/[controller]")]
   // [ApiController]
    public class CourseController : Controller
    {
        private readonly ICourseService courseService;

        public CourseController(ICourseService courseService)
        {
            this.courseService = courseService;
        }
        // GET: api/<CourseController>
        [HttpGet(ApiRoutes.Courses.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var all = await this.courseService.GetAllAsync();
            return Ok(all);
        }

        // GET api/<CourseController>/5
        [HttpGet(ApiRoutes.Courses.Get)]
        public async Task<IActionResult> Get(Guid id)
        {
            var course = await this.courseService.GetAsync(id);
            if (course is null)
                return BadRequest(course);

            return Ok(course);
        }

        // POST api/<CourseController>
        [HttpPost(ApiRoutes.Courses.Create)]
        public async Task<IActionResult> Create([FromBody] CreateCoursePostRequest createPostRequest)
        {
            try
            {
                var course = new Course()
                {
                    Id = new Guid(),
                    SubjectName = createPostRequest.SubjectName,
                    TeacherId = createPostRequest.TeacherId,
                    Description = createPostRequest.Description,
                    Price = createPostRequest.Price
                };

                var result = await this.courseService.AddAsync(course);

                var baseUri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
                var locationUri = baseUri + "/" + ApiRoutes.Courses.Get.Replace("{courseId}", course.Id.ToString());

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT api/<CourseController>/5
        [HttpPut(ApiRoutes.Courses.Update)]
        public async Task<IActionResult> Update(Guid courseId, [FromBody] CourseUpdateRequest courseUpdate)
        {
            //var userOwnsCourse = await this.courseService.UserOwnsCourseAsync(courseId, HttpContext.GetUserId());

            //if (!userOwnsCourse)
            //    return BadRequest(new { Errors = "You have not access to this course" });

            var dbCourse = new Course()
            {
                Id = courseId,
                SubjectName = courseUpdate.SubjectName,
                Description = courseUpdate.Description,
                Price = courseUpdate.Price,
                TeacherId = courseUpdate.TeacherId
            };

            var result = await this.courseService.UpdateAsync(dbCourse);

            if (!result) return NotFound();

            return Ok(dbCourse);
        }

        // DELETE api/<CourseController>/5
        [HttpDelete(ApiRoutes.Courses.Delete)]
        public async Task<IActionResult> Delete(Guid courseId)
        {
            //var userOwnsPost = await this.courseService.UserOwnsCourseAsync(courseId, HttpContext.GetUserId());

            //if (!userOwnsPost)
            //    return BadRequest(new { error = "You don't have access to this course" });\
            try
            {
                var deleted = await this.courseService.DeleteAsync(courseId);

                if (deleted)
                    return NoContent();

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

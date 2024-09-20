using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineSchoolCrm.Extensions;
using Service.Services.CourseService;


namespace OnlineSchoolCrm.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Route("api/[controller]")]
    //[ApiController]
    public class CourseController : Controller
    {
        private readonly ICourseService courseService;

        public CourseController(ICourseService courseService)
        {
            this.courseService = courseService;
        }
        // GET: api/<CourseController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var all = await this.courseService.GetAllAsync();
            return (IActionResult)all;
        }

        // GET api/<CourseController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var course = await this.courseService.GetAsync(id);
            return (IActionResult)course;
        }

        // POST api/<CourseController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Course createPostRequest)
        {
            var userOwnsCourse = await this.courseService.UserOwnsCourseAsync(createPostRequest.Id, HttpContext.GetUserId());

            return Ok();
        }

        // PUT api/<CourseController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        // DELETE api/<CourseController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid courseId)
        {
            var userOwnsPost = await this.courseService.UserOwnsCourseAsync(courseId, HttpContext.GetUserId());
            if (!userOwnsPost)
                return BadRequest(new { error = "You don't have access to this course" });

            var deleted = await this.courseService.DeleteAsync(courseId);

            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
}

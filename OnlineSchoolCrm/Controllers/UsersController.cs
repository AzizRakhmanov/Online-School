using AutoMapper;
using DAL.DataAccess;
using DAL.IRepository;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Dto;
using Service.Extensions;
using Service.Options;
using Service.Services.UserService;

namespace OnlineSchoolCrm.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SchoolDb _context;
        private IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ISchoolRepository<User> _repository;

        public UsersController(SchoolDb context,
            IUserService userService
           , IMapper mapper
           , ISchoolRepository<User> repository)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
            _repository = repository;
        }


        [HttpGet(ApiRoutes.Users.GetAll)]
        public async Task<IEnumerable<UserForResultDto>> AllUsers()
        {
            return await _userService.RetrieveAllAsync(p => p.Id != Guid.Empty);
        }

        [HttpGet(ApiRoutes.Users.Get)]
        public async Task<ActionResult<UserForResultDto>> GetUser(Guid id)
        {
            var user = await _userService.RetrieveAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPut(ApiRoutes.Users.Update)]
        public async Task<IActionResult> UpdateAsync(UserForCreationDto user)
        {
            // var userOwns = await _userService.UserOwnPostAsync(user.Id, new Guid(HttpContext.GetUserId()));


            try
            {
                var dbUser = await _userService.RetrieveAsync(user.Id);


                if (dbUser is null)
                    return NotFound();

                await _userService.Update(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost(ApiRoutes.Users.Create)]
        public async Task<ActionResult> CreateAsync([FromBody] UserForCreationDto user)
        {
            user.UserId = HttpContext.GetUserId();

            await _userService.AddAsync(user);

            var baseUri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUri + $"/" + ApiRoutes.Users.GetAll.Replace("userId", user.Id.ToString());

            return Ok(user);
         //   return CreatedAtAction(locationUri, user);
        }

        [HttpDelete(ApiRoutes.Users.Delete)]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _userService.RetrieveAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.Delete(id);

            return NoContent();
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}

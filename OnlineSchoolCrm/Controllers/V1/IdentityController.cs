using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Service.Options;
using Service.Services.IdentityService;

namespace OnlineSchoolCrm.Controllers.V1
{
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;
        public IdentityController(IIdentityService identityService)
        {
            this._identityService = identityService;
        }

        /// <summary>
        /// Registration for User
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            var authResponse = await this._identityService.RegisterAsync(request.Email, request.Password);

            if (!ModelState.IsValid)
            {
                return BadRequest(
                    new AuthFailedResponse()
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
            }

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse()
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse()
            {
                Token = authResponse.Token
            });
        }


        /// <summary>
        /// Logging for User
        /// </summary>
        /// <param name="userLoginRequest"></param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest userLoginRequest)
        {
            var authResponse = await this._identityService.LoginAsync(userLoginRequest.Email, userLoginRequest.Password);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse()
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse()
            {
                Token = authResponse.Token
            });
        }
    }
}

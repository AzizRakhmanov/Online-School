using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts.Requests;
using Service.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationResult = Service.Contracts.Responses.AuthenticationResult;

namespace Service.Services.IdentityService
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        public IdentityService(UserManager<IdentityUser> userManager,
            JwtSettings jwtSettings)
        {
            this._userManager = userManager;
            this._jwtSettings = jwtSettings;
        }
        public async Task<AuthenticationResult> RegisterAsync(UserRegistrationRequest request)
        {
            var existingUser = await this._userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                return new AuthenticationResult
                {
                    Errors = new List<string>
                    {
                        "User with this email address already exists"
                    }
                };
            }

            var newUser = new IdentityUser()
            {
                UserName = request.Email,
                Email = request.Email
            };

            var createdUser = await this._userManager.CreateAsync(newUser, request.Password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = createdUser.Errors.Select(e => e.Description)
                };
            }

            return GenerateTokenForUser(newUser);

        }


        public AuthenticationResult GenerateTokenForUser(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this._jwtSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email,user.Email),
                    new Claim("id",user.Id)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);



            return new AuthenticationResult
            {
                Token = tokenHandler.WriteToken(token),
                Success = true
            };
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await this._userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User does not exist" }
                };
            }

            var userHasRightPassword = await this._userManager.CheckPasswordAsync(user, password);

            if (!userHasRightPassword)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "Password does not match with user's password" }
                };
            }

            return GenerateTokenForUser(user);
        }
    }
}
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts;
using Service.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service.Services.IdentityService
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings jwtSettings;

        public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtSettings)
        {
            this._userManager = userManager;
            this.jwtSettings = jwtSettings;
        }


        public AuthenticationResult GenerateAuthenticationResultForUser(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email,user.Email),
                    new Claim("id",user.Id)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),

            };
        }


        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
            var existingUser = await this._userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User this email address already exists" }
                };
            }

            var newUser = new IdentityUser()
            {
                Email = email,
                UserName = email
            };


            var userHasValidPassword = await this._userManager.CheckPasswordAsync(existingUser, password);

            if (!userHasValidPassword)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User/password combination is not correct" }
                };
            }

            return GenerateAuthenticationResultForUser(existingUser);
        }


        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await this._userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new AuthenticationResult()
                {
                    Errors = new[] {"User does not exist"}
                };
            }

            var userHasValidPassword = await this._userManager.CheckPasswordAsync(user,password);

            if (!userHasValidPassword) {
                return new AuthenticationResult()
                {
                    Errors = new[] {"User/password combination is wrong"}
                };
            }
            return GenerateAuthenticationResultForUser(user);
        }
    }
}

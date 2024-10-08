﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Design;
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
        private readonly TokenValidationParameters _tokenValidationParameters;

        public IdentityService(UserManager<IdentityUser> userManager,
            JwtSettings jwtSettings,
            TokenValidationParameters tokenValidationParamaters)
        {
            this._userManager = userManager;
            this.jwtSettings = jwtSettings;
            this._tokenValidationParameters = tokenValidationParamaters;    
        }

        /// <summary>
        /// Method for generating token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public AuthenticationResult GenerateAuthenticationResultForUser(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email,user.Email),
                    new Claim("id",user.Id)
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                //Expires = DateTime.UtcNow.Add(jwtSettings.TokenLifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token)
            };
        }


        /// <summary>
        /// Registeris the new user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
            var existingUser = await this._userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User with this email address already exists" }
                };
            }

            var newUser = new IdentityUser()
            {
                Email = email,
                UserName = email
            };

            var createdUser = await this._userManager.CreateAsync(newUser, password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult()
                {
                    Errors = createdUser.Errors.Select(x => x.Description)
                };
            }

            return GenerateAuthenticationResultForUser(newUser);
        }


        /// <summary>
        /// Logins the user if user exists
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>AuthenticationResult</returns>
        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await this._userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new AuthenticationResult()
                {
                    Errors = new[] { "User does not exist" }
                };
            }

            var userHasValidPassword = await this._userManager.CheckPasswordAsync(user, password);

            if (!userHasValidPassword)
            {
                return new AuthenticationResult()
                {
                    Errors = new[] { $"{email}/{password} combination is not found" }
                };
            }
            return GenerateAuthenticationResultForUser(user);
        }

        public Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
        {
            return null;
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            return new ClaimsPrincipal();
            //var tokenHandler = new JwtSecurityTokenHandler();

            //try
            //{
            //    var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);

            //    if (!IsJwtWithValidSecurityALgorithm(validatedToken))
            //    {
            //        return null;
            //    }

            //    return principal;
            //}
            //catch
            //{
            //    return null;
            //}

            //return null;
        }

        private bool IsJwtWithValidSecurityALgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase);
        }
    }
}

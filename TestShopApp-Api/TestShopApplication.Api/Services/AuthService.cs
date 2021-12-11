using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TestShopApplication.Dal.Models;
using TestShopApplication.Dal.Repositories;
using TestShopApplication.Api.Models;

namespace TestShopApplication.Api.Services
{
    public class AuthService
    {
        private static SymmetricSecurityKey SecurityKey { get; set; }
        private static string Issuer;

        private IAuthRepository AuthRepository { get; }
        

        public AuthService(IAuthRepository authRepository, IConfiguration configuration)
        {
            AuthRepository = authRepository;
            SecurityKey ??= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            Issuer ??= configuration["JWT:ValidIssuer"];
        }

        public async Task<(bool userExists, string token, string firstName, string lastName)> Authenticate(LoginDataPresentation loginData)
        {
            try
            {
                UserSecurityDetails userSecurityDetails = await AuthRepository.GetUserSecurityDetails(loginData.Username);
                (bool userExists, string token, string firstName, string lastName) result = (false, null, null, null);

                if (userSecurityDetails != null
                    && !string.IsNullOrWhiteSpace(userSecurityDetails.PasswordHash)
                    && BCrypt.Net.BCrypt.Verify(loginData.Password, userSecurityDetails.PasswordHash))
                {
                    var authClaims = new List<Claim>
                    {
                        new Claim("name", userSecurityDetails.UserId.ToString()),
                        new Claim("jti", Guid.NewGuid().ToString()),
                    };
                    foreach (var role in Enum.GetValues(typeof(ShopAppUserRole)).Cast<ShopAppUserRole>().Where(r => r <= userSecurityDetails.Role))
                    {
                        authClaims.Add(new Claim("role", role.ToString()));
                    }
                    var jwtToken = new JwtSecurityToken(
                        issuer: Issuer,
                        audience: "*",
                        expires: DateTime.Now.AddHours(1),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256));
                    result.token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                    result.userExists = true;
                    result.firstName = userSecurityDetails.FirstName;
                    result.lastName = userSecurityDetails.LastName;
                }
                return result;
            }
            catch (Exception ex)
            {
                return (false, null, null, null);
            }
        }
    }
}

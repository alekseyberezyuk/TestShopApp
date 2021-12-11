using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestShopApplication.Api.Models;
using TestShopApplication.Api.Services;

namespace TestShopApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthService AuthService { get; }

        public AuthController(AuthService authService)
        {
            AuthService = authService;
        }

        /// <summary>
        /// Authenticates user
        /// </summary>
        /// <param name="loginData">A json object with username and password</param>
        /// <returns>A result of the operation</returns>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(AuthResponsePresentation), 200)]
        public async Task<IActionResult> Authenticate([FromBody] LoginDataPresentation loginData)
        {
            var (userExists, token, firstName, lastName) = await AuthService.Authenticate(loginData);
            var isSuccess = userExists && !string.IsNullOrWhiteSpace(token);
            AuthResponsePresentation authResult = new();

            if (isSuccess)
            {
                authResult = new AuthResponsePresentation
                {
                    IsSuccess = isSuccess,
                    Token = token,
                    FirstName = firstName,
                    LastName = lastName
                };
            }
            return Ok(authResult);
        }
    }
}

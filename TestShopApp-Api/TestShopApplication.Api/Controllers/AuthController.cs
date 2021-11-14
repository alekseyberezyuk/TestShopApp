using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        public async Task<IActionResult> Authenticate([FromBody] LoginData loginData)
        {
            var (userExists, token) = await AuthService.Authenticate(loginData);
            var isSuccess = userExists && !string.IsNullOrWhiteSpace(token);
            var authResult = new AuthResponse
            {
                IsSuccess = isSuccess,
                Token = isSuccess ? token : null
            };
            return Ok(authResult);
        }

        [HttpGet]
        [Route("testuser")]
        [Authorize(Roles = "User")]
        public ActionResult TestUser()
        {
            return Ok();
        }

        [HttpGet]
        [Route("testadmin")]
        [Authorize(Roles = "Admin")]
        public ActionResult TestAdmin()
        {
            return Ok();
        }
    }
}

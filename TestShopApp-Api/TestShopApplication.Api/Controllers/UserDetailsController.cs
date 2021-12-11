using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestShopApplication.Api.Models;
using TestShopApplication.Api.Services;

namespace TestShopApplication.Api.Controllers
{
    [Route("userdetails")]
    [Authorize(Roles = "User")]
    public class UserDetailsController : Controller
    {
        private UserDetailsService UserDetailsService { get; }

        public UserDetailsController(UserDetailsService userDetailsService)
        {
            UserDetailsService = userDetailsService;
        }

        [HttpGet]
        [Route("{userId}")]
        [Produces("application/json")]
        public IActionResult Get([FromRoute] Guid userId)
        {
            if (userId != Guid.Empty)
            {
                UserDetailsRepresentation userDetails = UserDetailsService.GetUserDetails(userId);
                return Ok(userDetails);
            }
            return BadRequest();
        }

        [HttpPut]
        [Produces("application/json")]
        public IActionResult Put([FromBody] UserDetailsRepresentation userDetails)
        {
            if (userDetails.Id != Guid.Empty && ModelState.IsValid)
            {
                bool result = UserDetailsService.UpdateUserDetails(userDetails);
                return Ok(result);
            }
            return BadRequest();
        }
    }
}

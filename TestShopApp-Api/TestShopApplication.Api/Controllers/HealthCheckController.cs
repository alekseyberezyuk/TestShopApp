using System;
using Microsoft.AspNetCore.Mvc;

namespace TestShopApplication.Api.Controllers
{
    [Route("healthcheck")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}

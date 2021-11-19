using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestShopApplication.Api.Services;

namespace TestShopApplication.Api.Controllers
{
    [Route("orders")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class OrdersController : Controller
    {
        private readonly OrdersService _ordersService;
        public OrdersController(OrdersService ordersService)
        {
            _ordersService = ordersService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            if (userId == Guid.Empty)
                return null;
            return Ok(await _ordersService.GetAll(userId));
        }

        private Guid GetUserId()
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == "name");
            if(userClaim == null)
                return Guid.Empty;
            return Guid.Parse(userClaim.Value);
        }
    }
}
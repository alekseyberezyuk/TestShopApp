using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestShopApplication.Api.Models;
using TestShopApplication.Api.Services;

namespace TestShopApplication.Api.Controllers
{
    [Route("order")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly OrdersService _ordersService;
        public OrderController(OrdersService ordersService)
        {
            _ordersService = ordersService;
        }
        [HttpGet("{order_id}")]
        public async Task<IActionResult> GetAll(
            [FromRoute(Name = "order_id")] Guid orderId)
        {
            return Ok(await _ordersService.GetAllItems(orderId));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(IEnumerable<OrderItem> orderItems)
        {
            var userId = GetUserId();
            return Ok(await _ordersService.CreateOrder(userId, orderItems));
        }

        [HttpPatch("{order_id}")]
        public async Task<IActionResult> CancelOrder(
            [FromRoute(Name = "order_id")] Guid orderId)
        {
            return Ok(await _ordersService.CancelOrder(orderId));
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
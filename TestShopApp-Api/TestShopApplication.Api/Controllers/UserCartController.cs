using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestShopApplication.Api.Models;
using TestShopApplication.Api.Services;
using TestShopApplication.Dal.Common;

namespace TestShopApplication.Api.Controllers
{
    [Route("cart")]
    [Authorize(Roles = "User")]
    public class UserCartController : Controller
    {
        private readonly UserCartService _userCartService;
        public UserCartController(UserCartService service)
        {
            _userCartService = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetShoppingCart()
        {
            var userId = GetUserId();
            return Ok(await _userCartService.GetShoppingCart(userId));
        }

        [HttpPost("add/{itemId}/{quantity}")]
        public async Task<IActionResult> AddToCart([FromRoute(Name = "itemId")]string itemId, 
            [FromRoute(Name = "quantity")]int quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest(new Response<Guid>
                {
                    Success = false,
                    Errors = new List<string> {"The quantity must be more than 0."}
                });
            }
            var userId = GetUserId();
            return Ok(await _userCartService.AddItemToCart(new ShoppingCartItem
            {
                ItemId = Guid.Parse(itemId),
                UserId = userId,
                Quantity = quantity
            }));
        }

        [HttpDelete("delete/{itemId}/{quantity}")]
        public async Task<IActionResult> DeleteFromCart([FromRoute(Name = "itemId")] Guid itemId,
            [FromRoute(Name = "quantity")] int quantity)
        {
            if (quantity <= 0)
                return BadRequest(new Response<Guid>
                {
                    Success = false,
                    Errors = new List<string> {"The quantity must be more than 0."}
                });
            
            var userId = GetUserId();
            return Ok(await _userCartService.DeleteItemToCart(new ShoppingCartItem
            {
                ItemId = itemId,
                UserId = userId,
                Quantity = quantity
            }));
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
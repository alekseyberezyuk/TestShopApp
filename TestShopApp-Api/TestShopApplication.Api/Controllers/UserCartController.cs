using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestShopApplication.Dal.Models;
using TestShopApplication.Api.Models;
using TestShopApplication.Api.Services;

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

        [HttpPost("add/{itemId}")]
        public async Task<IActionResult> AddToCart([FromRoute(Name = "itemId")]string itemId)
        {
            var userId = GetUserId();
            return Ok(await _userCartService.AddItemToCart(new ShoppingCartItem
            {
                ItemId = itemId,
                UserId = userId.ToString(),
                Quantity = 1
            }));
        }

        [HttpPatch("update/{itemId}/{quantity}")]
        public async Task<IActionResult> UpdateCartItem([FromRoute(Name = "itemId")] string itemId,
            [FromRoute(Name = "quantity")] int quantity)
        {
            if (quantity <= 0 || quantity > 30)
            {
                return BadRequest(new Response<Guid>
                {
                    Success = false,
                    Errors = new List<string> { "The quantity must be more than 0 and less than 30." }
                });
            }
            var userId = GetUserId();
            return Ok(await _userCartService.UpdateItemQuantity(new ShoppingCartItem
            {
                ItemId = itemId,
                UserId = userId.ToString(),
                Quantity = quantity
            }));
        }

        [HttpDelete("delete/{itemId}")]
        public async Task<IActionResult> DeleteFromCart([FromRoute(Name = "itemId")] Guid itemId)
        {
            var userId = GetUserId();
            return Ok(await _userCartService.DeleteItemFromCart(new ShoppingCartItem
            {
                ItemId = itemId.ToString(),
                UserId = userId.ToString(),
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
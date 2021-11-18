using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestShopApplication.Api.Services;
using TestShopApplication.Dal.Common;

namespace TestShopApplication.Api.Controllers
{
    [Route("items")]
    [ApiController]
    public class ItemsController : Controller
    {
        private readonly ItemsService _itemsService;
        public ItemsController(ItemsService itemsService)
        {
            _itemsService = itemsService;
        }
        
        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAll([FromQuery]int? category = null)
        {
            return Ok(await _itemsService.GetAll(category));
        }
        
        [HttpGet("{itemId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Get([FromRoute] Guid itemId)
        {
            return Ok(await _itemsService.GetById(itemId));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateItem([FromBody]ItemWithCategory item)
        {
            var result = await _itemsService.Create(item);
            if(result.Success)
                return Ok(result);

            return BadRequest(result);
        }
        
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateItem([FromBody]ItemWithCategory item)
        {
            var result = await _itemsService.Update(item);
            if(result.Success)
                return Ok(result);

            return BadRequest(result);
        }
        
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteItem([FromQuery] Guid itemId)
        {
            return Ok(await _itemsService.Delete(itemId));
        }
    }
}
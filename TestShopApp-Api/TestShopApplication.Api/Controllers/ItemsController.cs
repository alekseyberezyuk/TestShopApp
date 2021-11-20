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
        public async Task<IActionResult> GetAll([FromQuery] decimal minPrice = 0, [FromQuery] decimal? maxPrice = null, [FromQuery] string categories = null)
        {
            if (minPrice < 0 || maxPrice <= 0 || minPrice >= maxPrice || !ValidateCategories(categories))
            {
                return BadRequest();
            }
            var categoryList = ParseCategories(categories);
            
            if (categoryList == null)
            {
                return BadRequest();
            }
            var filterParameters = new FilterParameters(minPrice, maxPrice, categoryList);
            return Ok(await _itemsService.GetAll(filterParameters));
        }

        private IList<string> ParseCategories(string categories)
        {
            string[] categoryList = categories?.Split('\u002C') ?? Array.Empty<string>();

            if (categoryList.Distinct().Count() != categoryList.Length || categoryList.Any(c => !int.TryParse(c, out var x) || x <= 0))
            {
                return null;
            }
            return categoryList;
        }

        [HttpGet("{itemId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Get([FromRoute] Guid itemId)
        {
            return Ok(await _itemsService.GetById(itemId));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateItem([FromBody]ItemModel item)
        {
            var result = await _itemsService.Create(item);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("{itemId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateItem([FromRoute(Name = "itemId")]Guid itemId, [FromBody]ItemModel item)
        {
            var result = await _itemsService.Update(itemId, item);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{itemId}")]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteItem([FromRoute(Name = "itemId")] Guid itemId)
        {
            return Ok(await _itemsService.Delete(itemId));
        }

        private static bool ValidateCategories(string categories)
        {
            return categories == null
                || (categories != ""
                    && !categories.StartsWith(',')
                    && !categories.EndsWith(',')
                    && categories.All(c => c == ',' || char.IsDigit(c))
                    && !categories.Where((c, i) => i > 0 && c == ',' && categories[i - 1] == ',').Any());
        }
    }
}
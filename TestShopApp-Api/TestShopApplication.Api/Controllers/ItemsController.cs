using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestShopApplication.Dal.Models;
using TestShopApplication.Api.Models;
using TestShopApplication.Api.Services;

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

        /// <summary>
        /// Gets items with optional parameters to filter them
        /// </summary>
        /// <param name="minPrice">A minimum allowed price</param>
        /// <param name="maxPrice">A maximum allowed price</param>
        /// <param name="categories">A list of comma separated category ids eg. 1,2,3</param>
        /// <param name="searchParam">A text search parameter</param>
        /// <returns>All the items or only those that match the condition in filter</returns>
        [HttpGet]
        [Authorize(Roles = "User")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll(
            [FromQuery] decimal minPrice = 0,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] string categories = null,
            [FromQuery] string searchParam = null)
        {
            if (minPrice < 0 || maxPrice <= 0 || minPrice >= maxPrice || !ValidateCategories(categories))
            {
                return BadRequest();
            }
            var categoryList = ParseCategories(categories);

            if (categoryList == null || (searchParam != null && string.IsNullOrWhiteSpace(searchParam)))
            {
                return BadRequest();
            }
            var filterParameters = new FilterParameters(minPrice, maxPrice, categoryList);
            var items = await _itemsService.GetAll(filterParameters);

            if (searchParam != null)
            {
                items = items.Where(i =>
                {
                    var selection = $"{i.CategoryName}:{i.Name}:{i.Description}".ToLowerInvariant();
                    return selection.Contains(searchParam.ToLowerInvariant());
                });
            }
            return Ok(items);
        }

        /// <summary>
        /// Gets a specific item by id
        /// </summary>
        /// <param name="itemId">An item id</param>
        /// <returns>An item found</returns>
        [HttpGet("{itemId}")]
        [Authorize(Roles = "User")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromRoute] Guid itemId)
        {
            if (itemId == Guid.Empty)
            {
                return BadRequest();
            }
            return Ok(await _itemsService.GetById(itemId));
        }

        /// <summary>
        /// Adds a new item
        /// </summary>
        /// <param name="item">Adds a new item</param>
        /// <returns>Status code and Id of the new item if success</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddItem([FromBody]ItemPresentation item)
        {
            var result = await _itemsService.Create(item);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        /// Updates an existing item
        /// </summary>
        /// <param name="itemId">Existing item's id</param>
        /// <param name="item">An updated item to update the existing item with</param>
        /// <returns>Status code and the updated item if success</returns>
        [HttpPut("{itemId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateItem([FromRoute(Name = "itemId")]Guid itemId, [FromBody]ItemPresentation item)
        {
            var result = await _itemsService.Update(itemId, item);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        /// Removes an item
        /// </summary>
        /// <param name="itemId">The id of the item that needs to be removed</param>
        /// <returns>True when the item deleted, false when the item was not found</returns>
        [HttpDelete("{itemId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteItem([FromRoute(Name = "itemId")] Guid itemId)
        {
            return Ok(await _itemsService.Delete(itemId));
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestShopApplication.Dal.Models;
using TestShopApplication.Shared.ApiModels;
using TestShopApplication.Api.Services;
using TestShopApplication.Api.Validators;

namespace TestShopApplication.Api.Controllers
{
    [Route("items")]
    [ApiController]
    public class ItemsController : Controller
    {
        private ItemsService ItemsService { get; }
        private IItemsValidator Validator { get; }

        public ItemsController(ItemsService itemsService, IItemsValidator validator)
        {
            ItemsService = itemsService;
            Validator = validator;
        }

        /// <summary>
        /// Gets items with optional parameters to filter them
        /// </summary>
        /// <param name="minPrice">A minimum allowed price</param>
        /// <param name="maxPrice">A maximum allowed price</param>
        /// <param name="categories">A list of comma separated category ids eg. 1,2,3</param>
        /// <param name="searchParam">A text search parameter</param>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="itemsPerPage">How many items per page</param>
        /// <param name="orderBy">Which property use to order items. By default it orders by creation date in reverse order</param>
        /// <param name="includeThumbnails">Whether to return thumbnails</param>
        /// <returns>All the items or only those that match the condition in filter</returns>
        [HttpGet]
        [Authorize(Roles = "User")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ItemsPresentation), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] decimal minPrice = 0,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] string categories = null,
            [FromQuery] string searchParam = null,
            [FromQuery] int? pageNumber = null,
            [FromQuery] int? itemsPerPage = null,
            [FromQuery] OrderBy? orderBy = null,
            [FromQuery] bool? includeThumbnails = true)
        {
            var (isSuccess, validationError) = Validator.Validate(minPrice, maxPrice, categories, searchParam, itemsPerPage, pageNumber);

            if (!isSuccess)
            {
                return BadRequest(validationError);
            }
            var (parsedCategories, parseCategoriesErrorMsg) = ParseCategories(categories);

            if (parsedCategories == null)
            {
                return BadRequest(parseCategoriesErrorMsg);
            }
            var filterParameters = new FilterParameters(minPrice, maxPrice, parsedCategories, orderBy, includeThumbnails);
            var items = await ItemsService.GetAll(filterParameters);

            if (!string.IsNullOrWhiteSpace(searchParam))
            {
                items = items.Where(i =>
                {
                    var selection = $"{i.CategoryName}:{i.Name}:{i.Description}".ToLowerInvariant();
                    return selection.Contains(searchParam.ToLowerInvariant());
                });
            }
            var itemsPresentation = new ItemsPresentation
            {
                TotalItems = items.Count()
            };
            if (itemsPerPage > 0 && pageNumber > 0)
            {
                items = items.Skip((pageNumber.Value - 1) * itemsPerPage.Value).Take(itemsPerPage.Value);
            }
            itemsPresentation.Items = items;
            return Ok(itemsPresentation);
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
        public async Task<IActionResult> Get([FromRoute] Guid itemId)
        {
            if (itemId == Guid.Empty)
            {
                return BadRequest();
            }
            var item = await ItemsService.GetById(itemId);
            return item != null ? Ok() : NotFound();
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
        public async Task<IActionResult> AddItem([FromBody]ItemPresentation item)
        {
            var result = await ItemsService.Create(item);

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
        public async Task<IActionResult> UpdateItem([FromRoute(Name = "itemId")]Guid itemId, [FromBody]ItemPresentation item)
        {
            var result = await ItemsService.Update(itemId, item);
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
            return Ok(await ItemsService.Delete(itemId));
        }

        private (IList<string> result, string errorMsg) ParseCategories(string categories)
        {
            string[] categoryList = categories?.Split('\u002C') ?? Array.Empty<string>();

            if (categoryList.Distinct().Count() != categoryList.Length)
            {
                return (null, "Duplicate category ids");
            }
            if (categoryList.Any(c => !int.TryParse(c, out var x) || x == 0 || x > 1000 || (x > 0 && c.ToString()[0] == '0')))
            {
                return (null, "All category ids must be valid numbers from 1 to 1000");
            }
            return (categoryList, null);
        }


    }
}
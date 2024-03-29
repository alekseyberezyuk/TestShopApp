﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestShopApplication.Shared.ApiModels;
using TestShopApplication.Api.Services;

namespace TestShopApplication.Api.Controllers
{
    [Route("categories")]
    [Authorize(Roles = "User")]
    public class CategoriesController : Controller
    {
        private readonly CategoriesService _categoryService;

        public CategoriesController(CategoriesService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Produces("application/json")]
        public IActionResult Get()
        {
            IEnumerable<CategoryRepresentation> categories = _categoryService.GetCategories();
            return Ok(categories);
        }
    }
}

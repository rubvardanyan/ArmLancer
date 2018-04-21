using System;
using System.Collections.Generic;
using ArmLancer.API.Models.Requests;
using ArmLancer.API.Models.Responses;
using ArmLancer.Core.Interfaces;
using ArmLancer.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArmLancer.API.Controllers
{
    [Route("api/v1/categories")]
    public class CategoriesController : BaseController<Category, CategoryRequest>
    {
        private readonly ICategoryService _categoryService;
        
        public CategoriesController(IServiceProvider serviceProvider, ICategoryService categoryService) : base(serviceProvider)
        {
            _categoryService = categoryService;
        }
        
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetList(long? parentId)
        {
            var res = _categoryService.GetAllRecursive(parentId);
            return Ok (new DataResponse<IEnumerable<Category>>(res));
        }
    }
}
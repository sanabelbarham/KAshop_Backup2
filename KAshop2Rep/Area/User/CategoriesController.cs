using BLL.Service;
using KAshop2Rep.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KAshop2Rep.Area.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer _stringLocalizer;

        public CategoriesController(ICategoryService categoryService , IStringLocalizer<SharedResources> stringLocalizer)
        {
            _categoryService = categoryService;
            _stringLocalizer = stringLocalizer;
        }
        [HttpGet("")]
        public async Task <IActionResult> Index([FromQuery] string lang = "en")
        {
            var responce = await _categoryService.GetAllCategoriesForUser(lang);
            return Ok(new { message = _stringLocalizer["Success"].Value,responce });
       

        }


    }
}

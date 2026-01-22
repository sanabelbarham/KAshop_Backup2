using BLL.Service;
using DAL.DTO.Request;
using KAshop2Rep.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace KAshop2Rep.Area.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : ControllerBase
    {

        private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer _stringLocalizer;

        public CategoriesController(ICategoryService categoryService, IStringLocalizer <SharedResources> stringLocalizer)
        {
            _categoryService = categoryService;
            _stringLocalizer = stringLocalizer;
        }
        [HttpPost("")]
        public IActionResult Create(CategoryRequest categoryRequest)
        {
            var result = _categoryService.CreatCategoryAsync(categoryRequest);
            return Ok(new { message = _stringLocalizer["Success"].Value });
        }


        [HttpGet("")]
        public IActionResult Index()
        {
            var responce = _categoryService.GetAllCategoriesForAdmin();
            return Ok(new { message = _stringLocalizer["Success"].Value, responce });


        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result =await  _categoryService.DeleteCategoryAsync(id);

            if (result is null)
            {
                if(result.Message.Contains("no user with this id"))
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }
       
            return Ok(new { message = _stringLocalizer["Success"].Value });
        }

        [HttpPatch("toggle-status/{id}")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var result = await _categoryService.ToggleStatus(id);
            if (result is null)
            {
                if (result.Message.Contains("Not Found"))
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }
            return Ok(result);
        }

    

          [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] CategoryRequest request)
        {
            var result = await _categoryService.UpdateCategoryAsync(id, request);
            if (result is null)
            {
                if (result.Message.Contains("Not Found"))
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}

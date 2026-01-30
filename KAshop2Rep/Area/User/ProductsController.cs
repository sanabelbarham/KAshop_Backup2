using BLL.Service;
using KAshop2Rep.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KAshop2Rep.Area.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _ProductService;
        private readonly IStringLocalizer _stringLocalizer;

        public ProductsController(IProductService productService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            _ProductService = productService;
            _stringLocalizer = stringLocalizer;
        }
        [HttpGet("")]
        public async Task <IActionResult> Index([FromQuery] string lang = "en")
        {
            var responce =await _ProductService.GetAllProductsForUser(lang);
            return Ok(new { message = _stringLocalizer["Success"].Value, responce });


        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Index([FromRoute] int id, [FromQuery] string lang = "en")
        {
            var responce = await _ProductService.GetAllProductsDetailsForUser(id,lang);
            return Ok(new { message = _stringLocalizer["Success"].Value, responce });


        }
    }
}

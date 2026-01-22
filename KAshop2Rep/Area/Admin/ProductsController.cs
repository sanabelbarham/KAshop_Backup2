using BLL.Service;
using DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KAshop2Rep.Area.Admin
{
    [Route("api/Admin/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IStringLocalizer<ProductsController> _localizer;


        public ProductsController(IProductService productService, IStringLocalizer<ProductsController> localizer
)
        {
            _productService = productService;
            _localizer = localizer;
        }


        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] ProductRequest request)
        {
            var responce = await _productService.CreateProduct(request);
            return Ok(new { message = _localizer["Success"].Value,responce }); 
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var responce = _productService.GetAllProductsForAdmin();
            return Ok(new { message = _localizer["Success"].Value, responce });


        }

    }




}

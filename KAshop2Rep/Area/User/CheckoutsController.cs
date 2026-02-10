using BLL.Service;
using DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace KAshop2Rep.Area.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CheckoutsController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutsController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost("")]
        public async Task<IActionResult> Payment([FromBody] CheckoutRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var responce = await _checkoutService.ProcessPaymentAsync(request, userId);
            if (responce.Success == "false")
            {
                return BadRequest(responce);

            }
            return Ok(responce);
        }


        [HttpGet("success")]
        [AllowAnonymous]
        public async Task<IActionResult> Success([FromQuery] string session_id)
        {
            var responce = await _checkoutService.HandleSuccessAsync(session_id);
         
            if (responce.Success == "false")
            {
                return BadRequest(responce);
            }

            return Ok(responce);
          
        }
    }
}

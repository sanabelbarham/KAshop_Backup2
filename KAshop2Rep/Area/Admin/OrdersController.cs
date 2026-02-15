using BLL.Service;
using DAL.DTO.Request;
using DAL.Models;
using KAshop2Rep.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Stripe.Climate;

namespace KAshop2Rep.Area.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]


    public class OrdersController : ControllerBase
    {

        private readonly IOrderService _orderService;
        private readonly IStringLocalizer _stringLocalizer;

        public OrdersController(IOrderService orderService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            _orderService = orderService;
            _stringLocalizer = stringLocalizer;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetOrders(
    [FromQuery] OrderStatusEnum status = OrderStatusEnum.Pending)
        {
            var orders = await _orderService.GetOrderAsync(status);

            return Ok(orders);
        }
        [HttpPatch("{orderId}")]
      public async Task<IActionResult> UpdateStatus(
              [FromRoute] int orderId,
            [FromBody] UpdateOrderStatusRequest request)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, request.Status);

            if (result.Success=="false")
                return BadRequest(result);

            return Ok(result);
        }



    }
}

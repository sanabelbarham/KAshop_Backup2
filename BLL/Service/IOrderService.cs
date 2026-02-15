using DAL.DTO.Response;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
  public  interface IOrderService
    {
        Task<List<OrderResponce>> GetOrderAsync(OrderStatusEnum status);
        Task<BaseResponce> UpdateOrderStatusAsync( int orderId,OrderStatusEnum newStatus);
        Task<Order?> GetOrderByIdAsync( int orderId);
    }
}

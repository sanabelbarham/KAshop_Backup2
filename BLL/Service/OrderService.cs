using DAL.DTO.Response;
using DAL.Models;
using DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository )
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<OrderResponce>> GetOrderAsync(OrderStatusEnum status)
        {
            var orders= _orderRepository.GetOrdersByStatusEnum(status);
            return  orders.Adapt<List<OrderResponce>>();

        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }

        public async Task<BaseResponce> UpdateOrderStatusAsync(int orderId, OrderStatusEnum newStatus)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            order.OrderStatus = newStatus;

            if (newStatus == OrderStatusEnum.Delivered)
            {
                order.PaymentStatus = PaymentStatusEnum.paid;
            }
            else if(newStatus == OrderStatusEnum.Cancelled)
            {
                if (order.OrderStatus == OrderStatusEnum.Shipped)
                {
                    return new BaseResponce
                    {
                        Success = "false",
                        Message = "cant cancle order"
                    };
                }
            }

            await _orderRepository.UpdateAsync(order);
            return new BaseResponce
            {
                Success = "true",
                Message = "order status updated"
            };
        }
    }
}

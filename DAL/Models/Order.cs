using DAL.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public enum OrderStatusEnum
    {
        Pending=1,
        Cancelled=2,
        Approved=3,
        Shipped=4,
        Delivered=5
    }
    public enum PaymentMethodEnum{
        cash=1,
        visa=2
     

    }
  public  class Order
    {
        public int Id { get; set; }
        public OrderStatusEnum OrderStatus { get; set; } = OrderStatusEnum.Pending;
        public DateTime OrderDate { get; set; }
        public DateTime ShippedDate { get; set; }

 
        public string? SessionId { get; set; }// is a number created by stripe for any paying attempt fail or success
        public string? PaymentId { get; set; }// is a number created by stripe for any paying attempt fail or success
        public decimal? AmountPaid { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        public List <OrderItem>  OrderItems{ get; set; }
    }
}

using DAL.DTO.Request;
using DAL.DTO.Response;
using DAL.Identity;
using DAL.Models;
using DAL.Repository;
using Microsoft.AspNetCore.Identity;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IProductRepository _productRepository;

        public CheckoutService(ICartRepository cartRepository,IOrderRepository orderRepository ,
            
            UserManager<ApplicationUser> userManager,IEmailSender emailSender,IOrderItemRepository orderItemRepository, IProductRepository productRepository
            )
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
            _emailSender = emailSender;
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
        }

      
        public async Task<CheckoutResponce> ProcessPaymentAsync(CheckoutRequest request, string userId)
        {
            var cartItems = await _cartRepository.GetUserCartAsync(userId);
            if (!cartItems.Any())
            {
                return new CheckoutResponce
                {
                    Success = "false",
                    Message = "cart is empty"
                };
            }
            decimal totoalPrice = 0;


            foreach (var CartItem in cartItems)
            {
                if(CartItem.Product.Quantity< CartItem.Count)
                {
                    return new CheckoutResponce
                    {
                        Success = "false",
                        Message = "not enough items in stock"
                    };
                }
                totoalPrice += CartItem.Product.Price * CartItem.Count;


            }


            Order order = new Order
            {
                UserId = userId,
                PaymentMethod = request.PaymentMethod,
                AmountPaid = totoalPrice,
                PaymentStatus=PaymentStatusEnum.unpaid
            };


            if (request.PaymentMethod == PaymentMethodEnum.visa)
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>(),


                    Mode = "payment",
                    SuccessUrl = $"https://localhost:7122/api/checkouts/success?session_id={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"https://localhost:7122/checkouts/cancel",
                    Metadata=new Dictionary<string, string>
                    {
                       { "UserId",userId},
                    }
                };


                foreach (var item in cartItems)
                {

                    options.LineItems.Add(new SessionLineItemOptions
                    {

                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "USD",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Translations.FirstOrDefault(t => t.Language == "en").Name,
                            },
                            UnitAmount = (long)item.Product.Price*100,
                        },
                        Quantity = item.Count,


                    }
                    );

                }

                var service = new SessionService();
                var session = service.Create(options);
                order.SessionId = session.Id;
                order.PaymentStatus = PaymentStatusEnum.paid;
                await _orderRepository.CreateAsync(order);

                return new CheckoutResponce
                {
                    Success = "true",
                    Message = "Payment session created",
                    Url = session.Url
                };
             
            }
            else if (request.PaymentMethod == PaymentMethodEnum.cash)
            {

                return new CheckoutResponce
                {
                    Success = "true",
                    Message = "cash",
                  
                };
            }
            else
            {
                return new CheckoutResponce
                {
                    Success = "false",
                    Message = "payment method is not correct"
                };
            }
        }


        public async Task<CheckoutResponce> HandleSuccessAsync(string sessionId)
        {
            var service = new SessionService();
            var session = service.Get(sessionId);
            var userId = session.Metadata["UserId"];

            var order = await _orderRepository.GetBySessionIdAsync(sessionId);
            order.PaymentId = session.PaymentIntentId;
            order.OrderStatus = OrderStatusEnum.Approved;
            await _orderRepository.UpdateAsync(order);


            var user = await _userManager.FindByIdAsync(userId);

            var cartItems = await _cartRepository.GetUserCartAsync(userId);
            var orderItems = new List<OrderItem>();
            var productUpdated = new List<(int productId, int quantity)>();
            foreach (var item in cartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    UnitPrice = item.Product.Price,
                    Quantity = item.Count,
                    TotalPrice = item.Product.Price * item.Count,
                };
                orderItems.Add(orderItem);
                productUpdated.Add((item.ProductId, item.Count));

             

            }
            await _productRepository.DecreaseQuantityAsync(productUpdated);
            await _orderItemRepository.CreateAsync(orderItems);
             


            await _emailSender.SendEmailAsync(user.Email, "Payment Successfull", "<h2> thank you </h2>");
            return new CheckoutResponce
            {
                Success = "true",
                Message = "Payment completed succefully"
            };
        }
    }
}

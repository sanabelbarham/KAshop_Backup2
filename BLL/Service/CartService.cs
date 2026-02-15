using DAL.DTO.Request;
using DAL.DTO.Response;
using DAL.Models;
using DAL.Repository;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class CartService : ICartService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;

        public CartService(IProductRepository productRepository,ICartRepository cartRepository)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
        }

        public async Task<BaseResponce> AddToCartAsync(string userid, AddToCartRequest request)
        {
            var result = await _productRepository.FindByIdAsync(request.ProductId);
            if (result == null)
            {
                return new BaseResponce
                {
                    Success = "false",
                    Message = "product not found"
                };
            }
            var cartItem = await _cartRepository.GetCartItemsAsync(userid, request.ProductId);
            var exsitingCount = cartItem?.Count ?? 0;

            if (result.Quantity< (exsitingCount + request.Count))
            {
                return new BaseResponce
                {
                    Success = "false",
                    Message = " no enough products in stouck"
                };

            }
            if(cartItem is not null)
            {
                cartItem.Count += request.Count;
                await _cartRepository.UpdateAsync(cartItem);


            }
            else
            {
                var cart = request.Adapt<Cart>();
                cart.UserId = userid;// we do this bcs the request does not have user id so aafter i change
                                     //the model to Cart i will add the userid manually to it 
                await _cartRepository.CreateAsync(cart);
            }

           
            return new BaseResponce
            {
                Success = "true",
                Message = "Product added successfuly"
            };

        }

        public async Task<CartSummeryResponce> GetUserCartAsync(string userid, string lang = "en")
        {
            var cartItems = await _cartRepository.GetUserCartAsync(userid);
            var item = cartItems.Select(c => new CartResponce
            {
                ProductId = c.ProductId,
                Count = c.Count,
                Price = c.Product.Price,
                ProductName = c.Product.Translations.FirstOrDefault(t => t.Language == lang).Name,


            }).ToList();
            return new CartSummeryResponce
            {
                Items = item
            };
              




        }
        public async Task<BaseResponce> ClearCartAsync(string userId)
        {
            await _cartRepository.ClearCartAsync(userId);
            return new BaseResponce
            {
                Success = "true",
                Message = "Cart cleared successfully"
            };
        }

        public async Task <BaseResponce>RemoveFromCartAsync(string userId,int productId)
        {
            var cartItems = await _cartRepository.GetCartItemsAsync(userId, productId);
            if(cartItems is null)
            {
                return new BaseResponce
                {
                    Success = "false",
                    Message = "product not found"
                };
            }
            await _cartRepository.DeleteAsync(cartItems);
            return new BaseResponce
            {
                Success = "true",
                Message = "item removed from cart"
            };


        }

        public async Task <BaseResponce>UpdateQuantityAsync(string userId,int productId,int count)
        {



            var cartItems = await _cartRepository.GetCartItemsAsync(userId, productId);
            var product = await _productRepository.FindByIdAsync(productId);

            if(count <= 0)
            {
                return new BaseResponce
                {
                    Success = "false",
                    Message = "not enough stuck"
                };
            }

            if (product.Quantity < count)
            {
                return new BaseResponce
                {
                    Success = "false",
                    Message = "not enough stuck"
                };

            }
            cartItems.Count = count;
            await _cartRepository.UpdateAsync(cartItems);
            return new BaseResponce
            {
                Success = "true",
                Message = "Quntity updated successfully"
            };

        }


    }
}

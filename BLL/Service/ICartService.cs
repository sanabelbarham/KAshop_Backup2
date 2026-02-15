using DAL.DTO.Request;
using DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
   public interface ICartService
    {
         Task<BaseResponce> AddToCartAsync(string userid, AddToCartRequest request);
         Task<CartSummeryResponce> GetUserCartAsync(string userid, string lang="en");
        Task<BaseResponce> ClearCartAsync(string userId);
        Task<BaseResponce> RemoveFromCartAsync(string userId, int productId);
        Task<BaseResponce> UpdateQuantityAsync(string userId, int productId, int count);
    }
}

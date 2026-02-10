using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
   public interface ICartRepository
    {

        Task<Cart> CreateAsync(Cart request);

        Task<List<Cart>> GetUserCartAsync(string userId);
        Task<Cart?> GetCartItemsAsync(string userId, int productId);
        Task<Cart> UpdateAsync(Cart cart);
        Task ClearCartAsync(string userId);



    }
}

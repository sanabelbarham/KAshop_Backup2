using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Cart> CreateAsync(Cart request)
        {
            await _context.Carts.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;

        }
     





     

        public async Task<List<Cart>> GetUserCartAsync(string userId)
        {
            return await _context.Carts.Where(x => x.UserId == userId)
                .Include(c => c.Product)//theis does not contain the product name since product name is inside the translation
                .ThenInclude(c => c.Translations)// so i must do include inside the previus include with the translation to get the product name
                .ToListAsync();


            // from cart get the user and product and from product get the product name from translation
            // i a was inside product >> _context.product 
            //then it is okay to do include translation .Include(c => c.Translations) only without the need of 2 includes
       
        }

        public async Task<Cart?> GetCartItemsAsync(string userId,int productId)
        {
            return await  _context.Carts.FirstOrDefaultAsync(
                c => c.ProductId == productId && c.UserId == userId

                );
           
        }
        public async Task <Cart> UpdateAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            _context.SaveChangesAsync();
            return cart;
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await _context.Carts.Where(c => c.UserId == userId).ToListAsync();
            _context.Carts.RemoveRange(cart);
            await _context.SaveChangesAsync();


        }

         public async Task DeleteAsync(Cart cart)
        {
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }

    }
}

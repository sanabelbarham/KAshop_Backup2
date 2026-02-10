using DAL.Data;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
   public class OrderItemRepository:IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(List<OrderItem> request)
        {
             _context.AddRange(request);
             _context.SaveChangesAsync();
        

        }
    }
}

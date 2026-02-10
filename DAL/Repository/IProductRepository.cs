using DAL.DTO.Response;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product product);
        Task<List<Product>> GetAllAsync();
         Task<Product> FindByIdAsync(int id);
        Task<bool> DecreaseQuantityAsync(List<(int productId, int quentity)> items);
        IQueryable<Product> Query();
    }
}

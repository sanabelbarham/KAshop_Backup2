using DAL.DTO.Request;
using DAL.DTO.Response;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category> CreateAsync(Category request);
        Task<Category> FindById(int id);
        Task DeleteCategoryAsync(Category category);
        Task<Category?> UpdateAsync(Category category);

    }
}

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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Category> CreateAsync(Category request)
        {
         await   _context.Categories.AddAsync(request);
         await   _context.SaveChangesAsync();
            return request;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            var category = await _context.Categories.Include(c => c.Translations).Include(c=>c.User).ToListAsync();
            return category;
        }

        public async Task<Category> FindById(int id)
        {
            return await _context.Categories.Include(c => c.Translations).FirstOrDefaultAsync(c => c.Id == id);

        }

        public async Task DeleteCategoryAsync(Category category)
        {
            _context.Categories.Remove(category);
           await _context.SaveChangesAsync();
        }
        public async Task<Category?> UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChangesAsync();
            return category;
        }
    }
}

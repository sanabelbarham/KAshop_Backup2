using DAL.DTO.Request;
using DAL.DTO.Response;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public interface ICategoryService
    {
        Task<List<CategoryResponce>> GetAllCategoriesForAdmin();
        Task<CategoryResponce> CreatCategoryAsync(CategoryRequest request);
        Task<BaseResponce> DeleteCategoryAsync(int id);
        Task<BaseResponce> ToggleStatus(int id);
        Task<BaseResponce> UpdateCategoryAsync(int id, CategoryRequest request);
        Task<List<CategoryUserResponce>> GetAllCategoriesForUser(string lang = "en");

    }
}

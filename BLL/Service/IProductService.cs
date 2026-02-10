using DAL.DTO.Request;
using DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public interface IProductService
    {
        Task<ProductResponce> CreateProduct(ProductRequest request);
        Task<List<ProductResponce>> GetAllProductsForAdmin();
        Task<List<ProductUserDetailsResponce>> GetAllProductsForUser(int id, string lang = "en");
        Task<PaginatedResponce<ProductUserResponce>> GetAllProductsForUser(string lang = "en",
              int page = 1, int limit = 3, string? search = null,
              int? categoryId = null, decimal? minPrice = null, decimal? maxPrice = null

              );
    }
}

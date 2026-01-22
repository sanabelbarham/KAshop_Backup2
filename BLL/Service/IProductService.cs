using DAL.DTO.Request;
using DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
   public  interface IProductService
    {
        Task<ProductResponce> CreateProduct(ProductRequest request);
        Task<List<ProductResponce>> GetAllProductsForAdmin();
    }
}

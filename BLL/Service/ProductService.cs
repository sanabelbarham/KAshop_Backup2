using DAL.DTO.Request;
using DAL.DTO.Response;
using DAL.Models;
using DAL.Repository;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService;

        public ProductService(IProductRepository productRepository, IFileService fileService)
        {
            _productRepository = productRepository;
            _fileService = fileService;


        }

        public async Task<ProductResponce> CreateProduct(ProductRequest request)
        {
            var product = request.Adapt<Product>();

            if (request.MainImage != null)
            {
                var imagePath = await _fileService.UploadAsync(request.MainImage);
                product.MainImage = imagePath;
            }

            if (request.SubImages != null)
            {
                product.SubImage = new List<ProductImage>();

                foreach (var file in request.SubImages)
                {
                    var imagePath = await _fileService.UploadAsync(file);

                    product.SubImage.Add(new ProductImage
                    {
                        ImageName = imagePath
                    });
                }
            }

            await _productRepository.AddAsync(product);

            var response = product.Adapt<ProductResponce>();

            response.SubImages = product.SubImage
                .Select(s => s.ImageName)
                .ToList();

            return response;


        }

        public async Task<List<ProductResponce>> GetAllProductsForAdmin()
        {
            var product = await _productRepository.GetAllAsync();

            var responce = product.Adapt<List<ProductResponce>>();
            return responce;


        }


        public async Task<PaginatedResponce<ProductUserResponce>> GetAllProductsForUser(string lang = "en",
            int page = 1, int limit = 3, string? search = null,
            int? categoryId = null, decimal? minPrice = null, decimal? maxPrice = null,
            string? sortBy=null, bool asc =true

            )
        {
            var query = _productRepository.Query();

            if (search is not null)
            {
                query = query.Where(p => p.Translations.Any(t => t.Language == lang && t.Name.Contains(search) || t.Description.Contains(search)));

            }
            if (categoryId is not null)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }
            if (minPrice is not null)
            {
                query = query.Where(p => p.Price >= minPrice);
            }

            if (maxPrice is not null)
            {
                query = query.Where(p => p.Price <= maxPrice);
            }

            if (sortBy is not null)
            {
                sortBy = sortBy.ToLower();
                if (sortBy == "price"){
                    query = asc? query.OrderBy(p => p.Price):query.OrderByDescending(p=>p.Price);

                }else if(sortBy == "name")
                {
                    query = asc ? query.OrderBy(p => p.Translations.FirstOrDefault(t => t.Language == lang).Name)
                  : query.OrderByDescending(p => p.Translations.FirstOrDefault(t => t.Language == lang).Name);
                    

                }
                else if (sortBy == "rate")
                {
                    query = asc ? query.OrderBy(p => p.Rate) : query.OrderByDescending(p => p.Rate);
                }
            }

            var totalCount =await query.CountAsync();
            query = query.Skip((page - 1) * limit).Take(limit);
             

            var responce = query.BuildAdapter().AddParameters("lang", lang).AdaptToType<List<ProductUserResponce>>();
            return new PaginatedResponce<ProductUserResponce>
            {
                TotalCount = totalCount,
                Page = page,
                Limit = limit,
                Data = responce
            };


        }

     

        public async Task<List<ProductUserDetailsResponce>> GetAllProductsForUser(int id, string lang = "en")
        {
            var product = await _productRepository.FindByIdAsync(id);
            var responce = product.BuildAdapter().AddParameters("lang", lang).AdaptToType<List<ProductUserDetailsResponce>>();
            return responce;
        }
    }
}

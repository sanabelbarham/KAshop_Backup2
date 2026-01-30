using DAL.DTO.Request;
using DAL.DTO.Response;
using DAL.Models;
using DAL.Repository;
using Mapster;
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


        public async Task<List<ProductUserResponce>> GetAllProductsForUser(string lang = "en")
        {
            var product = await _productRepository.GetAllAsync();

            var responce = product.BuildAdapter().AddParameters("lang", lang).AdaptToType<List<ProductUserResponce>>();


            return responce;


        }

        public async Task<ProductUserDetailsResponce> GetAllProductsDetailsForUser(int id,string lang="en")
        {
            var product = await _productRepository.FindByIdAsync(id);
            var responce = product.BuildAdapter().AddParameters("lang", lang).AdaptToType<ProductUserDetailsResponce>();
            return responce;
        }
    }
}

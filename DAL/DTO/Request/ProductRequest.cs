using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Request
{
 public class ProductRequest
    {
       public List<ProductsTranslationRequest> Translations { get; set; }
        public int Quantity { get; set; }

        public double Price { get; set; }
        public double Discount { get; set; }
        public int CategoryId { get; set; }
        public List <IFormFile> SubImages { get; set; }
        public IFormFile MainImage { get; set; }

    }
}

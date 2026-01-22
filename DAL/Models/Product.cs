using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Product:BaseModel
    {
        public double Rate { get; set; }
        public int Quantity { get; set; }
    
        public double  Price { get; set; }
        public double  Discount { get; set; }
        public string  MainImage { get; set; }
        public int  CategoryId { get; set; }
        public Category Category { get; set; }
        public List<ProductTranslations> Translations { get; set; }
        public List<ProductImage> SubImage { get; set; }

    }
}

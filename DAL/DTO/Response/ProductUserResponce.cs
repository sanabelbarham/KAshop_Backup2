using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.DTO.Response
{
    public class ProductUserResponce
    {
        public string Name { get; set; }
        public int Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status status { get; set; }
     
        public string MainImage { get; set; }
        public double Rate { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
       // public double Discount { get; set; }

   

    }
}

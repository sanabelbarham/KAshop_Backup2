using DAL.Identity;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.DTO.Response
{
  public  class ProductResponce
    {
        public int Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status status { get; set; }
        public string CreatedBy { get; set; }
        public string MainImage { get; set; }
        public List<string> SubImages { get; set; }
        public List<ProductTranslationResponce> Translations { get; set; }

    }
}

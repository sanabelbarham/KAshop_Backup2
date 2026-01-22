using DAL.DTO.Response;
using DAL.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mappster
{
   public static class MappsterConfig
    {

            public static void MappsterConfigRegister()
        {
            TypeAdapterConfig<Category, CategoryUserResponce>.NewConfig()
                .Map(des => des.Name, source => source.Translations
                .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                .Select(t => t.Name).FirstOrDefault());


            TypeAdapterConfig<Product, ProductResponce>.NewConfig().Map(des => des.MainImage, source => $"https://localhost:7122/Images/{source.MainImage}");
        }
    }
}

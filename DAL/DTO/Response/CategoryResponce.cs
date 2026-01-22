using DAL.Identity;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Response
{
  public  class CategoryResponce
    {
        public int Id { get; set; }
        public Status status { get; set; }
        public string CreatedBy { get; set; }
        public List<CategoryTranslationResponce> Translations { get; set; }
    }
}

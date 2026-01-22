using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Response
{
  public  class BaseResponce
    {
        public string Message { get; set; }
        public string Success { get; set; }
        public List<string>? Errors { get; set; }
    }
}

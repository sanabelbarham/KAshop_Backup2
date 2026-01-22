using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Response
{
   public class LoginResponce:BaseResponce
    {
       
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }


    }
}

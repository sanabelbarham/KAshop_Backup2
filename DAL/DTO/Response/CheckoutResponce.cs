using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Response
{
   public class CheckoutResponce:BaseResponce
    {
        public string? Url { get; set; }
        public string? PaymentId { get; set; }
    }
}

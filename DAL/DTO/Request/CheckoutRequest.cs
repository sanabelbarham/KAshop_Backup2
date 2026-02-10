using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.DTO.Request
{

   public class CheckoutRequest
    {
      [JsonConverter(typeof(JsonStringEnumConverter))]
 
        public PaymentMethodEnum PaymentMethod { get; set; }
    }
}

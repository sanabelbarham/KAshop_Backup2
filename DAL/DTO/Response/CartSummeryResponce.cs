using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Response
{
   public class CartSummeryResponce
    {
        //it shows list of products 
        public List<CartResponce> Items { get; set; }
        public decimal CartTotal => Items.Sum(i => i.Total);

    }
}

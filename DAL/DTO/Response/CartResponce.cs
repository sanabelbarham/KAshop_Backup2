using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Response
{
    public class CartResponce
    {
        //this is only shows one line// 0ne product
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal Total => Count * Price;
        public string ProductName { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Smq.Web.Models
{
    public class OrderDetailViewModel
    {
        public int OrderID { set; get; }
        public int ProductID { set; get; }
        public int Quantity { set; get; }
        public string ProductName { get; set; }
        public decimal TotalMoney { get; set; }
        public decimal Price { get; set; }
    }
}
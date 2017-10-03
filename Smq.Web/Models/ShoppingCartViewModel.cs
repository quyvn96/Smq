﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Smq.Web.Models
{
    [Serializable]
    public class ShoppingCartViewModel
    {
        public int ProductId { get; set; }
        public ProductViewModel Product { get; set; }
        public int Quantity { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITRoots.Models
{
    public class ProductVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int? InvoiceID { get; set; }

    }
}
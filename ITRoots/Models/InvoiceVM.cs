using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ITRoots.Models
{
    public class InvoiceVM
    {
        public int ID { get; set; }
        public int? UserID { get; set; }

        [Display(Name = "CreatedDate", ResourceType = typeof(Resources.Resource))]
        public System.DateTime CreatedDate { get; set; }

        public  UserVM User { get; set; }
        public  List<ProductVM> Products { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
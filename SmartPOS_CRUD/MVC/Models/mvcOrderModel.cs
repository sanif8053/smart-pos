using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class mvcOrderModel
    {
        public int OrderId { get; set; }
        public int ProdId { get; set; }
        public int EmpId { get; set; }
        public int CustId { get; set; }
        public Nullable<int> OrderQuantity { get; set; }
        public string OrderDate { get; set; }
        public string ImagePath { get; set; }
    }
}
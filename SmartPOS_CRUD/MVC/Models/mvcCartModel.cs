using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class mvcCartModel
    {
        public int CartId { get; set; }
        public int CartDetailId { get; set; }
        public Nullable<int> CartQuantity { get; set; }
        public Nullable<int> CartTotal { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> ImageId { get; set; }
    }
}
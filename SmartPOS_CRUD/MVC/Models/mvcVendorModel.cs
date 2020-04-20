using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class mvcVendorModel
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorEmail { get; set; }
        public Nullable<int> ShopId { get; set; }
        public Nullable<int> ImageId { get; set; }
    }
}
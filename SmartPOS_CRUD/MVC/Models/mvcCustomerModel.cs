using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class mvcCustomerModel
    {
        public int CustId { get; set; }
        public string CustName { get; set; }
        public string CustEmail { get; set; }
        public string CustCategory { get; set; }
        public Nullable<int> ShopId { get; set; }
    }
}
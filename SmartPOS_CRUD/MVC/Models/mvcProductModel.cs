using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MVC.Models
{
    public class mvcProductModel
    {
        public int ProdId { get; set; }
        public string ProdName { get; set; }
        public Nullable<int> ProdPrice { get; set; }
        public string ProdCatageory { get; set; }
        public Nullable<int> ProdQuantity { get; set; }
        public Nullable<System.DateTime> ProdDate { get; set; }
        public Nullable<int> ShopId { get; set; }

        public Nullable<int> ImageID { get; set; }

        
    }
}
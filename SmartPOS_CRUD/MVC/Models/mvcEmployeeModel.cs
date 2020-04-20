using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class mvcEmplyeeModel
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpEmail { get; set; }
        public string EmpUsername { get; set; }
        public Nullable<int> ShopId { get; set; }

    }
}
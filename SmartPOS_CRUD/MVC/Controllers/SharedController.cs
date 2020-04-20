using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class SharedController : Controller
    {
        // GET: Shared
        [ChildActionOnly]
        public ActionResult Navbar()
        {
            IEnumerable<mvcCartModel> carts;
            HttpResponseMessage response = GlobalHttp.WebApiClient.GetAsync("Carts").Result;
            carts = response.Content.ReadAsAsync<IEnumerable<mvcCartModel>>().Result;

            //return View("~/Views/Shared/_Navbar.cshtml", carts);
            return View("~/Views/Shared/_Navbar.cshtml",carts);
        }
    }
}
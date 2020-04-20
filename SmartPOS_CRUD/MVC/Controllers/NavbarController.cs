using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Models;
using System.Net.Http;

namespace MVC.Controllers
{
    public class NavbarController : Controller
    {
        // GET: Navbar
        public ActionResult Index()
        {
            IEnumerable<mvcCartModel> carts ;
            HttpResponseMessage response = GlobalHttp.WebApiClient.GetAsync("Carts").Result;
            carts = response.Content.ReadAsAsync<IEnumerable<mvcCartModel>>().Result;

            return View("~/Views/Shared/_Navbar.cshtml",carts);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Models;
using System.Net.Http;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            IEnumerable<mvcProductModel> products;
            HttpResponseMessage response1 = GlobalHttp.WebApiClient.GetAsync("Products").Result;
            products = response1.Content.ReadAsAsync<IEnumerable<mvcProductModel>>().Result;

            IEnumerable<mvcEmplyeeModel> employees;
            HttpResponseMessage response2 = GlobalHttp.WebApiClient.GetAsync("Employees").Result;
            employees = response2.Content.ReadAsAsync<IEnumerable<mvcEmplyeeModel>>().Result;

            IEnumerable<mvcCustomerModel> customers;
            HttpResponseMessage response3 = GlobalHttp.WebApiClient.GetAsync("Customers").Result;
            customers = response3.Content.ReadAsAsync<IEnumerable<mvcCustomerModel>>().Result;

            IEnumerable<mvcCartModel> carts;
            HttpResponseMessage response4 = GlobalHttp.WebApiClient.GetAsync("Carts").Result;
            carts = response4.Content.ReadAsAsync<IEnumerable<mvcCartModel>>().Result;

            List<int> list = new List<int>();
            int total_product = products.Count();
            int total_employee = employees.Count();
            int total_customer = customers.Count();
            int total_cart = carts.Count();
            list.Add(total_product);
            list.Add(total_employee);
            list.Add(total_customer);
            list.Add(total_cart);

            return View(list);
        }

        [Authorize]

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize]

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
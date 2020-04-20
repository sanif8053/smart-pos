using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Models;
using System.Net;
using System.Net.Http;

namespace MVC.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            IEnumerable<mvcOrderModel> orders;
            HttpResponseMessage response = GlobalHttp.WebApiClient.GetAsync("Orders").Result;
            orders = response.Content.ReadAsAsync<IEnumerable<mvcOrderModel>>().Result;
            return View(orders);
        }

        [HttpPost]
        public void AddOrder(mvcOrderModel order)
        {
            HttpResponseMessage response = GlobalHttp.WebApiClient.PostAsJsonAsync("Orders", order).Result;
            TempData["SuccessMessage"] = "Product Detail Added To Order History";

        }

        [HttpPost]

        public ActionResult AddOrderByValues(int prodId,string custName,int orderQuantity ,int imageId)
        {
            //Image
            ImageController imageController = new ImageController();

            HttpResponseMessage response = GlobalHttp.WebApiClient.GetAsync("Customers/Search/" + custName).Result;
            mvcCustomerModel customer = response.Content.ReadAsAsync<mvcCustomerModel>().Result;

            mvcOrderModel order=new mvcOrderModel();
            order.ProdId = prodId;
            order.ImagePath = imageController.ImageView(imageId);
            order.OrderQuantity = orderQuantity;
            order.CustId = customer.CustId;
            order.OrderDate = DateTime.Now.ToString();
            HttpResponseMessage Prodresponse = GlobalHttp.WebApiClient.PostAsJsonAsync("Orders", order).Result;

            return View("Index", "Product");
           
        }

    }
}
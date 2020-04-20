using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Models;
using System.Web;
using System.Net.Http;
using Microsoft.AspNet.Identity;

namespace MVC.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public int GetShop()
        {
            string userId = User.Identity.GetUserId();
            int shopId=0;
            IEnumerable<mvcShopModel> shops;
            HttpResponseMessage response = GlobalHttp.WebApiClient.GetAsync("Shops").Result;
            shops = response.Content.ReadAsAsync<IEnumerable<mvcShopModel>>().Result;
            foreach(var shop in shops)
            {
                if(shop.UserId == userId)
                {
                    shopId = shop.ShopId;
                }

            }

            return shopId;
        }
    }
}
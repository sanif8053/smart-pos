using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.IO;
using MVC.Models;
using MVC.Controllers;
using Microsoft.AspNet.Identity;

namespace MVC.Controllers
{
    public class ProductController : Controller
    {

        ShopController shop = new ShopController();
        // GET: Product
        [Authorize]
        public ActionResult Index()
        {
            IEnumerable<mvcProductModel> productList;
            HttpResponseMessage response = GlobalHttp.WebApiClient.GetAsync("Products").Result;
            productList = response.Content.ReadAsAsync<IEnumerable<mvcProductModel>>().Result;
            return View(productList);
        }

        [Authorize]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new mvcProductModel());
            }
            else
            {
                HttpResponseMessage response = GlobalHttp.WebApiClient.GetAsync("Products/" + id.ToString()).Result;
                mvcProductModel product = response.Content.ReadAsAsync<mvcProductModel>().Result;
                return View(product);
            }

        }
        [Authorize]
        public ActionResult Profile (int id)
        {
            IEnumerable<mvcProductModel> products;
            HttpResponseMessage response = GlobalHttp.WebApiClient.GetAsync("Products").Result;
            products = response.Content.ReadAsAsync <IEnumerable<mvcProductModel>> ().Result;
            return View(products);
        }

        [Authorize]
        public ActionResult AddImage()
        {
            var myAccount = new Account ("783418436741261", "qy8oddYcH1Hvdh92tPsqTXirVC0","tijarat-pos" );
            Cloudinary _cloudinary = new Cloudinary(myAccount);
            string imagepath = @"D:\Data\7th Semester\IPT\Project\WEB APP\SmartPOS_CRUD\MVC\Content\images\office.jpg";
            FileStream file = new FileStream(@"D:\Data\7th Semester\IPT\Project\WEB APP\SmartPOS_CRUD\MVC\Content\images\office.jpg",FileMode.Open,FileAccess.Read);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(@"C:\1.jpg")
            };
            //try
            //{
                var uploadResult = _cloudinary.Upload(uploadParams);
                TempData["SuccessMessage"] = "Image Uploaded";
            //}
            //catch(Exception ex)
            //{
              //  Console.WriteLine(ex);
                TempData["AlertMessage"] = "Image is not Uploaded";
            //}
            
            return RedirectToAction("Index");
            }

        [HttpPost]
        public ActionResult UpdateQuantity(int id,int reducequantity)
        {
            HttpResponseMessage response = GlobalHttp.WebApiClient.GetAsync("Products/" + id.ToString()).Result;
            mvcProductModel product = response.Content.ReadAsAsync<mvcProductModel>().Result;
            product.ProdQuantity -= reducequantity;
            HttpResponseMessage response2 = GlobalHttp.WebApiClient.PutAsJsonAsync("Products/" + id.ToString(),product).Result;

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddOrEdit(mvcProductModel product, Image file )
        {
            //int shopId = shop.GetShop();
            //product.ShopId = shopId;
            if (file.ImageFile != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(file.ImageFile.FileName);
                string extension = Path.GetExtension(file.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("tijarat") + extension;
                file.ImagePath = "/Images/Product/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Images/Product/"), fileName);
                file.ImageFile.SaveAs(fileName);
                using (Smart_POS2Image db = new Smart_POS2Image())
                {
                    db.Images.Add(file);
                    db.SaveChanges();
                    product.ImageID = db.Images.Max(image => image.ImageID);
                }
                ModelState.Clear();
            }
            else
            {
                TempData["AlertMessage"] = "Image Path is Null ";
            }

            

            if (product.ProdId == 0)
            {

                HttpResponseMessage request = GlobalHttp.WebApiClient.PostAsJsonAsync("Products", product).Result;
                TempData["SuccessMessage"] = product.ProdName + " Record  Added Succfully!!";
            }
            else
            {
                HttpResponseMessage response = GlobalHttp.WebApiClient.PutAsJsonAsync("Products/" + product.ProdId, product).Result;
                TempData["SuccessMessage"] = product.ProdName + " Record Updated Succfully!!";
            }

            return RedirectToAction("Index");
        }

        public ActionResult Search()
        {
            return View();
        }
        
        public ActionResult SearchByName(mvcProductModel prod)
        {

            HttpResponseMessage response = GlobalHttp.WebApiClient.GetAsync("Products/Search/" + prod.ProdName).Result;
            mvcProductModel product = response.Content.ReadAsAsync<mvcProductModel>().Result;
            TempData["SuccessMessage"] = product.ProdId + " is Id";
            return RedirectToAction("Index");
        }

        public string SearchName(int id)
        {
            mvcProductModel prod;
            HttpResponseMessage response = GlobalHttp.WebApiClient.GetAsync("Products/" + id.ToString()).Result;
            prod = response.Content.ReadAsAsync<mvcProductModel>().Result;
            return prod.ProdName;
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = GlobalHttp.WebApiClient.DeleteAsync("Products/" + id.ToString()).Result;

            TempData["AlertMessage"] = "Product Record Deleted Successfully!!";
            return RedirectToAction("Index");
        }
    }
}
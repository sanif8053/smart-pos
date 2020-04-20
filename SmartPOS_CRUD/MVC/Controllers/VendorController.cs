using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.IO;

namespace MVC.Controllers
{
    public class VendorController : Controller
    {
        // GET: Vendor
        [Authorize]
        public ActionResult Index()
        {
            IEnumerable<mvcVendorModel> vendors;
            HttpResponseMessage response = GlobalHttp.WebApiClient.GetAsync("Vendors").Result;
            vendors = response.Content.ReadAsAsync<IEnumerable<mvcVendorModel>>().Result;

            return View(vendors);
        }

        [Authorize]
        public ActionResult Profile(int id)
        {
            mvcVendorModel vendor;
            HttpResponseMessage response = GlobalHttp.WebApiClient.GetAsync("Vendors/" + id.ToString()).Result;
            vendor = response.Content.ReadAsAsync<mvcVendorModel>().Result;
            return View(vendor);
        }

        [Authorize]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new mvcVendorModel());
            }
            else
            {
                mvcVendorModel vendor;
                HttpResponseMessage response = GlobalHttp.WebApiClient.GetAsync("Vendors/" + id.ToString()).Result;
                vendor = response.Content.ReadAsAsync<mvcVendorModel>().Result;
                return View(vendor);
            }

        }

        [Authorize]
        [HttpPost]
        public ActionResult AddOrEdit(mvcVendorModel vendor, Image file)
        {
            //int shopId = shop.GetShop();
            //product.ShopId = shopId;
            if (file.ImageFile != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(file.ImageFile.FileName);
                string extension = Path.GetExtension(file.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("tijarat") + extension;
                file.ImagePath = "/Images/Vendor/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Images/Vendor/"), fileName);
                file.ImageFile.SaveAs(fileName);
                using (Smart_POS2Image db = new Smart_POS2Image())
                {
                    db.Images.Add(file);
                    db.SaveChanges();
                    vendor.ImageId = db.Images.Max(image => image.ImageID);
                }
                ModelState.Clear();
            }
            else
            {
                TempData["AlertMessage"] = "Image Path is Null ";
            }

            {
                if (vendor.VendorId == 0)
                {
                    HttpResponseMessage ressponse = GlobalHttp.WebApiClient.PostAsJsonAsync("Vendors", vendor).Result;
                    TempData["SuccessMessage"] = vendor.VendorName + " Added Successfully!!";
                }
                else
                {
                    HttpResponseMessage respone = GlobalHttp.WebApiClient.PutAsJsonAsync("Vendors/" + vendor.VendorId, vendor).Result;
                    TempData["SuccessMessage"] = vendor.VendorName + " Updated Successfully!!";
                }
                return RedirectToAction("Index");

            }
        }

        [Authorize]

        public ActionResult Delete(int id)
        {
            HttpResponseMessage respone = GlobalHttp.WebApiClient.DeleteAsync("Vendors/" + id.ToString()).Result;
            TempData["AlertMessage"] = "Vendor Deleted Successfully!!";
            return RedirectToAction("Index");
        }
    }
}
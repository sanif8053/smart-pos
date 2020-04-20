using MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public int Add(Image file)
        {
            int id=0;
            if (file.ImageFile != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(file.ImageFile.FileName);
                string extension = Path.GetExtension(file.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("tijarat") + extension;
                file.ImagePath = "~/Images/Product/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Images/Product/"), fileName);
                file.ImageFile.SaveAs(fileName);
                using(Smart_POS2Image db = new Smart_POS2Image())
                {
                    db.Images.Add(file);
                    db.SaveChanges();
                    id = db.Images.Max(image => image.ImageID);
                }
                ModelState.Clear();
            }
            else
            {
                TempData["AlertMessage"] = "Image Path is Null ";
            }
            // after successfully uploading redirect the user
            return id;
          
        }

        public string ImageView(int id)
        {
            Image imagefile = new Image();
            using (Smart_POS2Image db = new Smart_POS2Image())
            {
                imagefile = db.Images.Where(image => image.ImageID == id).FirstOrDefault();

            }
            return imagefile.ImagePath;
        }

    }
}
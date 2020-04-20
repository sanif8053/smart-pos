using MVC.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Syncfusion.Pdf;
using Syncfusion.HtmlConverter;

namespace MVC.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        [Authorize]
        public ActionResult Index()
        {
            IEnumerable<mvcCartModel> carts;
            HttpResponseMessage response = GlobalHttp.WebApiClient.GetAsync("Carts").Result;
            carts = response.Content.ReadAsAsync<IEnumerable<mvcCartModel>>().Result;
            
            return View(carts);
        }

        //Transation
        [Authorize]
        public ActionResult Modal()
        {
            mvcCustomerModel customer= new mvcCustomerModel();    
            return View(customer);
        }


        [HttpPost]
        public ActionResult AddModal(string CustName, string CustEmail, string CustCategory)
        {

            //Initialize the HTML to PDF converter 
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.WebKit);

            WebKitConverterSettings settings = new WebKitConverterSettings();

            //Set WebKit path
            settings.WebKitPath = @"D:/Data/7th Semester/IPT/Project/WEB APP/SmartPOS_CRUD/packages/Syncfusion.HtmlToPdfConverter.QtWebKit.AspNet.17.3.0.26/lib/QtBinaries";

            //Assign WebKit settings to HTML converter
            htmlConverter.ConverterSettings = settings;

            //Convert URL to PDF
            PdfDocument document = htmlConverter.Convert("http://localhost:62054/Cart/Invoice/");

            //Save and close the PDF document 
            document.Save("D:/Data/7th Semester/IPT/Project/WEB APP/SmartPOS_CRUD/MVC/Invoice/" + CustName+".pdf");

            document.Close(true);


            try
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("tijaratpos@gmail.com");
                msg.To.Add(new MailAddress(CustEmail));
                msg.Subject = "Thank You For Shopping";
                msg.Body = "Please check the Invoice Attached below.";
                Attachment attachment;
                attachment = new Attachment("D:/Data/7th Semester/IPT/Project/WEB APP/SmartPOS_CRUD/MVC/Invoice/" + CustName + ".pdf");
                attachment.Name = "Invoice.pdf";
                msg.Attachments.Add(attachment);

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("tijaratpos@gmail.com", "TijaratPOS000");
                smtpClient.Credentials = credentials;
                smtpClient.EnableSsl = true;
                smtpClient.Send(msg);

                mvcCustomerModel cust = new mvcCustomerModel();
                cust.CustName = CustName;
                cust.CustEmail = CustEmail;
                cust.CustCategory = CustCategory;
                HttpResponseMessage response = GlobalHttp.WebApiClient.PostAsJsonAsync("Customers", cust).Result;
                TempData["SuccessMessage"] = "Shopping Receipt Has Been Send To Customer";
            }
            catch(Exception ex)
            {
                TempData["AlertMessage"] = "Something Went Wrong Please Check Your Connection Or Enter Valid Email Address";
            }

            
            return RedirectToAction("Index");
        }


        [Authorize]
        [HttpPost]
        public ActionResult Update(int prodId,int newQuantity,int oldQuantity,int cartId,int imageId)
        {
            HttpResponseMessage prodresponse = GlobalHttp.WebApiClient.GetAsync("Products/" + prodId.ToString()).Result;
            mvcProductModel product = prodresponse.Content.ReadAsAsync<mvcProductModel>().Result;
            int actualQuantity=0;
            if (newQuantity > oldQuantity)
            {
                
                actualQuantity = newQuantity - oldQuantity;
                product.ProdQuantity -= actualQuantity;
            }
            else
            {
                 actualQuantity = oldQuantity - newQuantity;
                 product.ProdQuantity += actualQuantity;
            }
            
            if (product.ProdQuantity < actualQuantity)
            { 
                TempData["AlertMessage"] = "Only "+product.ProdQuantity+" are Available!!";
                return RedirectToAction("Index");
            }
            else
            {
                //CArt
                HttpResponseMessage cartresponse = GlobalHttp.WebApiClient.GetAsync("Carts/" + cartId.ToString()).Result;
                mvcCartModel cart = cartresponse.Content.ReadAsAsync<mvcCartModel>().Result;
                cart.CartQuantity = newQuantity;
                cart.CartTotal = newQuantity * product.ProdPrice;
                cart.ImageId = imageId;

                HttpResponseMessage response = GlobalHttp.WebApiClient.PutAsJsonAsync("Carts/" + cart.CartId.ToString(), cart).Result;

                //Product

               
                HttpResponseMessage productresponse = GlobalHttp.WebApiClient.PutAsJsonAsync("Products/" + product.ProdId.ToString(), product).Result;

                TempData["SuccessMessage"] = "Item Updated Successfully!!";
                return RedirectToAction("Index");
            }
            
            

        }

        [Authorize]
        [HttpPost]
        public ActionResult Add(int total,int prodId,int quantity, int imageId)
        {
            HttpResponseMessage prodresponse = GlobalHttp.WebApiClient.GetAsync("Products/" + prodId.ToString()).Result;
            mvcProductModel product = prodresponse.Content.ReadAsAsync < mvcProductModel> ().Result;
            
            mvcCartModel cart = new mvcCartModel();
            cart.CartDetailId = 1;
            cart.CartQuantity = quantity;
            cart.CartTotal = total;
            cart.ProductID = prodId;
            cart.ImageId = imageId;
            cart.ProductName = product.ProdName;
            HttpResponseMessage response = GlobalHttp.WebApiClient.PostAsJsonAsync("Carts", cart).Result;
            TempData["SuccessMessage"] = "Item Added in to Cart";
            return RedirectToAction("Index","Product");    
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = GlobalHttp.WebApiClient.DeleteAsync("Carts/" + id.ToString()).Result;
            TempData["AlertMessage"] = "Item Deleted from Cart successfully";
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult DeleteAll()
        {
            IEnumerable<mvcCartModel> carts;
            HttpResponseMessage response = GlobalHttp.WebApiClient.GetAsync("Carts").Result;
            carts = response.Content.ReadAsAsync<IEnumerable<mvcCartModel>>().Result;
            foreach ( mvcCartModel cart in carts)
            {
                HttpResponseMessage response2 = GlobalHttp.WebApiClient.DeleteAsync("Carts/" + cart.CartId.ToString()).Result;
            }
            TempData["AlertMessage"] = "All Item Deleted from Cart";
            return RedirectToAction("Index");
        }

       
        //  Invoice 
        public ActionResult Invoice()
        {
            IEnumerable<mvcCartModel> carts;
            HttpResponseMessage response = GlobalHttp.WebApiClient.GetAsync("Carts").Result;
            carts = response.Content.ReadAsAsync<IEnumerable<mvcCartModel>>().Result;

            return View(carts);
        }

        [Authorize]
        //Html to PDF
        public ActionResult PDF()
        {
            //Initialize the HTML to PDF converter 
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.WebKit);

            WebKitConverterSettings settings = new WebKitConverterSettings();

            //Set WebKit path
            settings.WebKitPath = @"D:/Data/7th Semester/IPT/Project/WEB APP/SmartPOS_CRUD/packages/Syncfusion.HtmlToPdfConverter.QtWebKit.AspNet.17.3.0.26/lib/QtBinaries";

            //Assign WebKit settings to HTML converter
            htmlConverter.ConverterSettings = settings;

            //Convert URL to PDF
            PdfDocument document = htmlConverter.Convert("http://localhost:62054/Cart/Invoice");

            //Save and close the PDF document 
            document.Save("D:/Data/7th Semester/IPT/Project/WEB APP/SmartPOS_CRUD/Output.pdf");

            document.Close(true);
            return RedirectToAction("Index");
        }

    }
}
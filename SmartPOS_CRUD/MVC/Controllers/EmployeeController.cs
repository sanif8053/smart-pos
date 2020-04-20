using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Models;
using System.Net.Http;

namespace MVC.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        [Authorize]
        public ActionResult Index()
        {
            IEnumerable<mvcEmplyeeModel> employeeList;
            HttpResponseMessage response = GlobalHttp.WebApiClient.GetAsync("Employees").Result;
            employeeList = response.Content.ReadAsAsync<IEnumerable<mvcEmplyeeModel>>().Result;
            return View(employeeList);
        }
        [Authorize]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new mvcEmplyeeModel());
            }
            else
            {
                HttpResponseMessage response = GlobalHttp.WebApiClient.GetAsync("Employees/" + id.ToString()).Result;
                mvcEmplyeeModel employee = response.Content.ReadAsAsync<mvcEmplyeeModel>().Result;
                return View(employee);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddOrEdit(mvcEmplyeeModel Employee)
        {
            if (Employee.EmpId == 0)
            {
                HttpResponseMessage response = GlobalHttp.WebApiClient.PostAsJsonAsync("Employees", Employee).Result;
                TempData["SuccessMessage"] = "Employee Record Added Successfully";
            }
            else
            {
                HttpResponseMessage response = GlobalHttp.WebApiClient.PutAsJsonAsync("Employees/" + Employee.EmpId, Employee).Result;
                TempData["SuccessMessage"] = "Employee Record Updated Successfully";
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = GlobalHttp.WebApiClient.DeleteAsync("Employees/" + id.ToString()).Result;
            TempData["AlertMessage"] = "Employee Record Deleted Successfully";
            return RedirectToAction("Index");
        }

    }
}
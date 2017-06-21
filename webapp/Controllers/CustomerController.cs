using LivellPayRoll.App_Helpers;
using LivellPayRoll.Infrastructure;
using LivellPayRoll.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LivellPayRoll.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Customers()
        {
            var CustomerList = db.Customer.Where(c => c.CompanyId == LoginUser.CompanyId ).ToList();
            ViewData.Model = CustomerList;
            return View();
        }
        public ActionResult AddCustomer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCustomer(Customer c)
        {
            c.Id = Guid.NewGuid();
            c.AddDate= DateTime.UtcNow;
            c.CompanyId= LoginUser.CompanyId;
            db.Customer.Add(c);
            db.SaveChanges();
            return RedirectToAction("Customers", "Customer", new { code = 1 , message = "Customer been successfully added!" });
        }
        public ActionResult Delete()
        {
            Guid id = new Guid(Request["Id"]);
            var CustomerList = db.Customer.Where<Customer>(c => c.Id.Equals(id)).ToList();
            Customer Cus = CustomerList.FirstOrDefault();
            db.Entry<Customer>(Cus).State= System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("code", 1);
            dic.Add("message", "Customer and jobs been successfully Deleted!");
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditCustomer()
        {
            Guid id = new Guid(Request["Id"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer CustomerInfo = db.Customer.Find(id);
            if (CustomerInfo == null)
            {
                return HttpNotFound();
            }
            ViewData["CustomerInfo"] = " [" + CustomerInfo.CustomerName + "]";
            return View(CustomerInfo);
        }
        [HttpPost]
        public ActionResult EditCustomer(Customer cus)
        {
            Customer EditCus = db.Customer.Where<Customer>(c => c.Id.Equals(cus.Id)).ToList().FirstOrDefault();
            EditCus.CustomerName = cus.CustomerName;
            EditCus.Attn = cus.Attn;
            EditCus.Telphone = cus.Telphone;
            EditCus.Fax = cus.Fax;
            EditCus.Email = cus.Email;
            EditCus.Address = cus.Address;
            EditCus.Remark = cus.Remark;

            db.Entry(EditCus).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Customers", "Customer", new { code = 1, message = "Customer been successfully edited!" });
        }
        public ActionResult CustomerJobs()
        {
            var CustomerList = db.Customer.Where(c => c.CompanyId == LoginUser.CompanyId).ToList();
            
            CustomerList.FirstOrDefault();
            ViewBag.Customers = CustomerList;
            return View();
        }
        [HttpPost]
        public ActionResult JobQuery()
        {
            int id = int.Parse(Request["Id"]);
            Job job = db.Job.Find(id);
            ViewData["EmployeeNum"] = job.Employee.Count();
            Dictionary<string, object> dicJob = new Dictionary<string, object>();
            dicJob.Add("JobId", job.JobId);
            dicJob.Add("JobName", job.JobName);
            dicJob.Add("Description", job.Description);
            dicJob.Add("status", job.status);
            return Json(dicJob, JsonRequestBehavior.AllowGet);
        }
        public ActionResult JobEmployeesList()
        {
            if (Request["Id"] == null)
            {
                return Redirect("CustomerJobs");
            }
            int id = int.Parse(Request["Id"]);
            Job job = db.Job.Where<Job>(j => j.JobId == id).ToList().FirstOrDefault();
            ViewBag.JobId = id;
            ViewBag.EmployeeNum = job.Employee.ToList().Count();
            return View(job.Employee.ToList());
        }
        public ActionResult RemoveJobEmployee()
        {
            int id = int.Parse(Request["JobId"]);
            Job job = db.Job.Find(id);
            Guid EmployeeId = new Guid(Request["EmployeeId"]);
            Employee emp= db.Employee.Find(EmployeeId);
            job.Employee.Remove(emp);
            db.SaveChanges();
            Dictionary<string, object> dicJob = new Dictionary<string, object>();
            dicJob.Add("code", 1);
            dicJob.Add("status", "success");
            return Json(dicJob, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCheckEmployeeList(int id)
        {
            Job job = db.Job.Where<Job>(j => j.JobId == id).ToList().FirstOrDefault();
            var Employees = db.Employee.Where<Employee>(e => e.CompanyId == LoginUser.CompanyId).ToList();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var e in Employees)
            {
                if(!job.Employee.Contains(e))
                {
                    items.Add(new SelectListItem() { Text = e.FName +" " + e.LName + " [" + e.Email + "]", Value = e.EmployeeId.ToString() , Selected = false });
                }
                else
                {
                    items.Add(new SelectListItem() { Text = e.FName + " " + e.LName + " [" + e.Email + "]", Value = e.EmployeeId.ToString() , Selected = true });
                }
                
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddJobEmployees(Guid[] Selected)
        {
            if (Request["JobId"] == null) {
                return Redirect("CustomerJobs");
            }
            int id = int.Parse(Request["JobId"]);
            Job job = db.Job.Where<Job>(j => j.JobId == id).ToList().Single();
            foreach (var e in job.Employee.ToList()) {
                job.Employee.Remove(e);
            }
            foreach (Guid EmployeeId in Selected)
            {
                Employee employee = db.Employee.Where<Employee>(e => e.EmployeeId == EmployeeId).ToList().Single();
                job.Employee.Add(employee);
            }
            db.SaveChanges();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("code", 1);
            dic.Add("status", "success");
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EditJob(Job job)
        {
            Job JobInfo = db.Job.Where<Job>(j => j.JobId==job.JobId ).ToList().FirstOrDefault();
            JobInfo.JobName = job.JobName;
            JobInfo.Description = job.Description;
            if (job.status == "on") JobInfo.status = "0";
            else JobInfo.status = "1";
            db.Entry(JobInfo).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("code", 1);
            dic.Add("status", "success");
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult addJob()
        {
            Guid CustomerId = new Guid(Request["CustomerId"]);
            string JobName = Request["JobName"];
            string Description = Request["Description"];
            Customer customer = db.Customer.Find(CustomerId);
            Job j = new Job() { JobName = JobName, Description= Description, status = "0", CompanyId= LoginUser.CompanyId, Customer= new List<Customer>() };
            db.Job.Add(j);
            j.Customer.Add(customer);
            db.SaveChanges();

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("code", 1);
            dic.Add("status", "success");
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteJob()
        {
            int id = int.Parse(Request["Id"]);
            var jobList = db.Job.Where<Job>(j => j.JobId==id).ToList();
            Job job = jobList.FirstOrDefault() ;
            db.Entry<Job>(job).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("code", 1);
            dic.Add("message", "Job and all job's employee been successfully Deleted!");
            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        private AppIdentityDbContext db
        {
            get
            {
                return HttpContext.GetOwinContext().Get<AppIdentityDbContext>();
            }
        }
        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
        private AppUser LoginUser
        {
            get
            {
                //AppUser user = HttpContext.User;
                AppUser user = UserManager.FindByName(HttpContext.User.Identity.Name);
                return user;
            }
        }


    }
}
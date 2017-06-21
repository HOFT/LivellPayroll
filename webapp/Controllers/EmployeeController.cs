using LivellPayRoll.Infrastructure;
using System.Web.Mvc;
using System.Linq;
using LivellPayRoll.Models;
using System.Collections.Generic;
using LivellPayRoll.App_Helpers;
using LivellPayRoll.Enum;
using System;
using System.Data.Entity.Validation;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Xml;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace LivellPayRoll.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult EmployeeList()
        {
            //AppIdentityDbContext db = (AppIdentityDbContext)System.Web.HttpContext.Current.Items["AppIdentityDbContext"];
            //Company company = (Company)System.Web.HttpContext.Current.Session["Company"];
            Dictionary<string, string> StatesDic = EnumHelper.GetEnumItemValueDesc(typeof(States));
            ViewData["StatesDic"] = StatesDic;
            Dictionary<string, string> StatusDic = EnumHelper.GetEnumItemValueDesc(typeof(Status));
            ViewData["StatusDic"] = StatusDic;
            var EmployeeList = db.Employee.Where<Employee>(e => e.CompanyId == LoginUser.CompanyId).ToList();
            ViewData.Model = EmployeeList;
            return View();
        }
        public ActionResult Add()
        {
            Company com = db.Company.Find(LoginUser.CompanyId);
            var EmployeeList = db.Employee.Where<Employee>(e => e.CompanyId == LoginUser.CompanyId).ToList();
            var JobList = db.Job.Where<Job>(j => j.CompanyId == LoginUser.CompanyId).ToList();
            Dictionary<string, object> StaList = EnumHelper.EnumListDic<States>("", "");
            Dictionary<string, object> TZList = TimeZones.DicTimeZones();
            ViewBag.StatesList = new SelectList(StaList, "value", "key");
            ViewBag.TimeZone = new SelectList(TZList, "value", "key");
            ViewData["EmployeeNum"] = EmployeeList.Count();
            ViewData["JobNum"] = JobList.Count();
            ViewBag.comTZ = com.TimeZone;
            ViewBag.comSta = com.State;

            var List102 = db.T102.Where<T102>(t => t.CompanyId == LoginUser.CompanyId).OrderBy(t => t.ItemId).ToList();
            ViewBag.List102 = List102;
            var List201 = db.T201.Where<T201>(t=>t.CompanyId== LoginUser.CompanyId && t.Type==1).OrderBy(t => t.Ord).ToList();
            ViewBag.List201 = List201;
            return View();
        }
        [HttpPost]
        public ActionResult Add(Employee em)
        {
            string ErrorMessage = string.Empty;
            object jsonObj;
            string jsonStr = string.Empty;
            JavaScriptSerializer json = new JavaScriptSerializer();
            em.EmployeeId = Guid.NewGuid();
            Employee Emp = ReplenishEmployee(em);
            if (em.DefaultJob != 0)
            {
                var j = db.Job.Where(t => t.JobId == em.DefaultJob).ToList();
                Emp.Job = j;
            }
            AppUser user = UserManager.FindByEmail(em.Email);
            if (user == null)
            {
                try
                {
                    AppUser EmpUser = new AppUser { Email = em.Email, UserName = em.Email, CompanyId = LoginUser.CompanyId, TimeZone = Emp.TimeZone, PayRollUser = em.FName + " " + em.LName };
                    UserManager.Create(EmpUser,"123456");
                    user = UserManager.FindByEmail(em.Email);
                    Emp.AppUser = user;
                    db.Employee.Add(Emp);
                    
                    if (!UserManager.IsInRole(user.Id, "Employee"))
                    {
                        UserManager.AddToRole(user.Id, "Employee");
                    }
                    //jsonObj = string.Format("code = {0}, status = {1}, message = {2}", 1, "success", "success add");
                    jsonObj = new { code = 1, status = "success", message = "success add" };
                } catch (DbEntityValidationException dbEx){
                    foreach (var validationErrors in dbEx.EntityValidationErrors){
                        foreach (var validationError in validationErrors.ValidationErrors){
                            System.Diagnostics.Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                            ErrorMessage = string.Format("{0}:{1}\r\n", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                    //jsonObj = string.Format("code = {0}, status = {1}, message = {2}", 2, "error", ErrorMessage);
                    jsonObj = new { code = 2, status = "error", message = ErrorMessage };
                    jsonStr = json.Serialize(jsonObj);
                    return Content(jsonStr);
                }
            }else{
                //jsonObj = string.Format("code = {0}, status = {1}, message = {2}", 3, "error", "This email has been registered");
                jsonObj = new { code = 3, status = "error", message= "This email has been registered" };
            }
            jsonStr = json.Serialize(jsonObj);
            return Content(jsonStr);
        }
        [HttpPost]
        public ActionResult Delete()
        {
            string EmpID = Request["Id"];
            Guid Eid = new Guid(EmpID);
            string ErrorMessage = string.Empty;
            try
            {
                var Employee = db.Employee.Where<Employee>(e => e.EmployeeId.Equals(Eid)).Single();
                AppUser user = UserManager.FindByEmail(Employee.Email);
                if (user != null)
                {
                    UserManager.Delete(user);
                }
                db.Entry<Employee>(Employee).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("code", 1);
            dic.Add("message", "Delete Successful");
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit()
        {
            Guid EmpID = new Guid(Request["Id"]);
            if (Request["Id"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var EmployeeList = db.Employee.Where<Employee>(e => e.CompanyId == LoginUser.CompanyId).ToList();
            var JobList = db.Job.Where<Job>(j => j.CompanyId == LoginUser.CompanyId).ToList();
            Dictionary<string, object> StaList = EnumHelper.EnumListDic<States>("", "");
            Dictionary<string, object> TZList = TimeZones.DicTimeZones();
            ViewBag.StatesList = new SelectList(StaList, "value", "key");
            ViewBag.TimeZone = new SelectList(TZList, "value", "key");
            ViewData["EmployeeNum"] = EmployeeList.Count();
            ViewData["JobNum"] = JobList.Count();
            var List102 = db.T102.Where<T102>(t => t.CompanyId == LoginUser.CompanyId).OrderBy(t=>t.ItemId).ToList();
            ViewBag.List102 = List102;
            var List201 = db.T201.Where<T201>(t => t.CompanyId == LoginUser.CompanyId && t.Type == 1).OrderBy(t => t.Ord).ToList();
            ViewBag.List201 = List201;
            if (EmpID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employeeInfo = db.Employee.Find(EmpID);
            if (employeeInfo == null)
            {
                return HttpNotFound();
            }
            ViewData["employeeInfo"] = "&nbsp;&nbsp;<i class='fa fa-user'></i>&nbsp;" + employeeInfo.FName+" ["+ employeeInfo.Email+ "]";
            return View(employeeInfo);
        }
        [HttpPost]
        public ActionResult Edit(Employee em)
        {
            string ErrorMessage = string.Empty;
            object jsonObj;
            string jsonStr = string.Empty;
            JavaScriptSerializer json = new JavaScriptSerializer();
            Employee Emp = ReplenishEmployee(em);
            if (em.DefaultJob != 0) {
                var j = db.Job.Where(t => t.JobId == em.DefaultJob).ToList();
                Emp.Job = j;
            }
            try
            {
                db.Entry(Emp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                jsonObj = new { code = 1, status = "success", message = "Employee Save Successful!" };
             }catch (DbEntityValidationException dbEx)
             {
                 foreach (var validationErrors in dbEx.EntityValidationErrors)
                 {
                     foreach (var validationError in validationErrors.ValidationErrors)
                     {
                        System.Diagnostics.Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        ErrorMessage = string.Format("{0}:{1}\r\n", validationError.PropertyName, validationError.ErrorMessage);
                      }
                 }
                 jsonObj = new { code = 2, status = "error", message = ErrorMessage };
                 jsonStr = json.Serialize(jsonObj);
                 return Content(jsonStr);
             }
            jsonStr = json.Serialize(jsonObj);
            return Content(jsonStr);
        }
        public JsonResult GetJob(string id) {
            JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();
            var customer = db.Customer.Where<Customer>(e => e.CompanyId == LoginUser.CompanyId).ToList();
            List<JobList> dic = new List<JobList>();
            foreach (var c in customer) {
                List<Jobs> JL = new List<Jobs>();
                Jobs jobs = new Jobs();
                foreach (var j in c.Job) {
                    JL.Add(new Jobs { JobId=j.JobId, JobName=j.JobName });
                }
                dic.Add(new JobList { CustomerName=c.CustomerName , Jobs= JL });
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        public JsonResult StateTax(int StaId) {
            string StaCode= States.GetName(typeof(States), StaId);
            string XmlFileName = Server.MapPath(@"~/App_Helpers/DataTableXml.xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(XmlFileName);
            XmlNode root = xmlDoc.SelectSingleNode("NewDataSet/St[ST100='"+StaCode+"']");
            string TitelCode = root.SelectSingleNode("ST100").InnerText;
            bool F109 = Convert.ToBoolean(root.SelectSingleNode("ST104").InnerText);
            bool F111 = Convert.ToBoolean(root.SelectSingleNode("ST113").InnerText);
            bool F112 = Convert.ToBoolean(root.SelectSingleNode("ST114").InnerText);
            bool F113 = Convert.ToBoolean(root.SelectSingleNode("ST115").InnerText);

            bool F120 = Convert.ToBoolean(root.SelectSingleNode("ST105").InnerText);
            bool F122 = Convert.ToBoolean(root.SelectSingleNode("S111").InnerText);

            bool flag11 = Convert.ToBoolean(root.SelectSingleNode("ST106").InnerText);
            bool flag12 = Convert.ToBoolean(root.SelectSingleNode("ST112").InnerText);
            bool flag4 = Convert.ToBoolean(root.SelectSingleNode("ST111").InnerText);
            bool flag6 = Convert.ToBoolean(root.SelectSingleNode("ST107").InnerText);
            bool flag7 = Convert.ToBoolean(root.SelectSingleNode("ST108").InnerText);
            bool flag9 = Convert.ToBoolean(root.SelectSingleNode("ST109").InnerText);
            bool flag8 = Convert.ToBoolean(root.SelectSingleNode("ST110").InnerText);
            string text1;
            if (root.SelectSingleNode("ST116") != null)
            {
                text1 = root.SelectSingleNode("ST116").InnerText.ToString().Trim();
            }
            else {
                text1 = "";
            }
            string text2;
            if (root.SelectSingleNode("ST117") != null)
            {
                text2 = root.SelectSingleNode("ST117").InnerText.ToString().Trim();
            }
            else {
                text2 = "";
            }
            bool flag = false;
            if ((((((((flag11 | flag12) | flag4) | flag6) | flag7) | flag9) | flag8) | TitelCode == "CT") | (TitelCode == "NJ"))
            {
                flag = true;
            }
            bool F110 = flag;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("TitelCode", TitelCode);
            dic.Add("F110", F110);
            dic.Add("F109", F109);
            dic.Add("F111", F111);
            dic.Add("F112", F112);
            dic.Add("F113", F113);
            dic.Add("F120", F120);
            dic.Add("F122", F122);
            dic.Add("text1", text1);
            dic.Add("text2", text2);
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FilingStatus(string StaCode) {
            string XmlFileName = Server.MapPath(@"~/App_Helpers/DataTableXml.xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(XmlFileName);
            XmlNode root = xmlDoc.SelectSingleNode("NewDataSet/St[ST100='" + StaCode + "']");
            bool flag11 = Convert.ToBoolean(root.SelectSingleNode("ST106").InnerText);
            bool flag12 = Convert.ToBoolean(root.SelectSingleNode("ST112").InnerText);
            bool flag4 = Convert.ToBoolean(root.SelectSingleNode("ST111").InnerText);
            bool flag6 = Convert.ToBoolean(root.SelectSingleNode("ST107").InnerText);
            bool flag7 = Convert.ToBoolean(root.SelectSingleNode("ST108").InnerText);
            bool flag9 = Convert.ToBoolean(root.SelectSingleNode("ST109").InnerText);
            bool flag8 = Convert.ToBoolean(root.SelectSingleNode("ST110").InnerText);

            List<SelectListItem> items = new List<SelectListItem>();
            if (StaCode == "AB")
            {
                items = FilingStatusDate(false);
            }
            else if (StaCode == "AC")
            {
                items = FilingStatusDate(false);
            }
            else if (StaCode == "CT")
            {
                items = FilingStatusDate(false);
            }
            else if (StaCode == "NJ")
            {
                items = FilingStatusDate(true);
            }
            else {
                items = FilingStatusDate(flag12, flag11, flag4, flag6, flag7, flag9, flag8);
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCounty(string StaCode) {
            string XmlFileName = Server.MapPath(@"~/App_Helpers/DataTableXml.xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(XmlFileName);
            XmlNodeList list = xmlDoc.SelectNodes("NewDataSet/"+ StaCode);
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (XmlNode r in list)
            {
                string P104 = r.SelectSingleNode("P104").InnerText;
                string P103 = r.SelectSingleNode("P103").InnerText;
                items.Add(new SelectListItem() { Text = P104, Value = P103 });

            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        private Employee ReplenishEmployee(Employee em)
        {
            em.F99 = 0;
            em.F103 = DateTime.UtcNow;
            em.F104 = DateTime.UtcNow;
            em.F105 = DateTime.UtcNow;
            em.HourlyPay1 = "";
            em.HourlyPay2 = "";
            em.HourlyPay3 = "";
            em.HourlyPay4 = "";
            em.is1099Employee = 0;
            em.CompanyId = LoginUser.CompanyId;
            return em;
        }
        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
        private AppIdentityDbContext db
        {
            get
            {
                return HttpContext.GetOwinContext().Get<AppIdentityDbContext>();
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
        private List<SelectListItem> FilingStatusDate(bool blnAtoE) {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "A", Value = "A" });
            items.Add(new SelectListItem() { Text = "B", Value = "B" });
            items.Add(new SelectListItem() { Text = "C", Value = "C" });
            items.Add(new SelectListItem() { Text = "D", Value = "D" });
            items.Add(new SelectListItem() { Text = "E", Value = "E" });
            if (blnAtoE) {
                items.Add(new SelectListItem() { Text = "F", Value = "F" });
            }
            return items;
        }
        private List<SelectListItem> FilingStatusDate(bool flag12, bool flag11, bool flag4, bool flag6, bool flag7, bool flag9, bool flag8)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            if (flag12)
            {
                items.Add(new SelectListItem() { Text = "Zero Exemptions", Value = "O" });
            }
            if (flag11)
            {
                items.Add(new SelectListItem() { Text = "Single", Value = "S" });
            }
            if (flag4)
            {
                items.Add(new SelectListItem() { Text = "Head of Houshold", Value = "H" });
            }
            if (flag6)
            {
                items.Add(new SelectListItem() { Text = "Married", Value = "M" });
            }
            if (flag7)
            {
                items.Add(new SelectListItem() { Text = "Married (Dual Incomes)", Value = "B" });
            }
            if (flag9)
            {
                items.Add(new SelectListItem() { Text = "Married (Filing Seperate)", Value = "M" });
            }
            if (flag8)
            {
                items.Add(new SelectListItem() { Text = "Married (Filing Joint)", Value = "J" });
            }
            return items;
        }
    }
}
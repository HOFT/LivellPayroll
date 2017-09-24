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
using System.Threading.Tasks;
using System.Data;
using System.Configuration;

namespace LivellPayRoll.Controllers
{
    [Authorize]
    [CustomAuthorize]
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult EmployeeList()
        {
            AppUser user = LoginUser;
            Dictionary<string, string> StatesDic = EnumHelper.GetEnumItemDesc(typeof(States));
            Dictionary<string, string> StatusDic = EnumHelper.GetEnumItemValueDesc(typeof(Status));
            int Year = TimeHelper.GetLocalTime(DateTime.UtcNow, Convert.ToDouble(user.TimeZone)).Year;
            DateTime startTime = TimeHelper.GetUTCTime(new DateTime(Year, 1, 1), Convert.ToDouble(user.TimeZone));
            DateTime endTime = TimeHelper.GetUTCTime(startTime.AddMonths(12), Convert.ToDouble(user.TimeZone));

            var EmployeeList = db.Employee.Where<Employee>(e => e.CompanyId == LoginUser.CompanyId).OrderBy(e=>e.UserRole).ToList();
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("EmployeeId");
            dataTable.Columns.Add("FName");
            dataTable.Columns.Add("LName");
            dataTable.Columns.Add("SSN");
            dataTable.Columns.Add("Phone");
            dataTable.Columns.Add("Email");
            dataTable.Columns.Add("State");
            dataTable.Columns.Add("Role");
            dataTable.Columns.Add("Status");
            dataTable.Columns.Add("CreateTime");
            dataTable.Columns.Add("UserId");
            dataTable.Columns.Add("CapHours");
            dataTable.Columns.Add("TotalHours");
            dataTable.Columns.Add("Percent");
            
            foreach (var e in EmployeeList) {
                DataRow dr = dataTable.NewRow();
                dr["EmployeeId"] = e.EmployeeId;
                dr["FName"] = e.FName;
                dr["LName"] = e.LName;
                dr["SSN"] = e.SSN;
                dr["Phone"] = e.Phone;
                dr["Email"] = e.Email;
                dr["State"] = StatesDic[e.State];
                dr["Role"] = e.UserRole;
                dr["Status"] = StatusDic[e.F124.ToString()];
                dr["CreateTime"] = TimeHelper.GetLocalTime(e.F103 , double.Parse(user.TimeZone));
                dr["UserId"] = e.AppUserId;
                double CapHours = 0;
                double PTOCapHours = GetStoragePTOHours(e.EmployeeId, Year, user, 0);
                double VacCapHours = GetStoragePTOHours(e.EmployeeId, Year, user, 1);
                //if (PTOCapHours >= VacCapHours) { CapHours = PTOCapHours; } else { CapHours = VacCapHours; }
                CapHours = PTOCapHours;
                dr["CapHours"] = CapHours;
                float total = 0;
                var ATJs = db.AccrualTimeJournal.Where(t => t.EmployeeId == e.EmployeeId && t.Type == 0 && t.Date >= startTime && t.Date <= endTime).ToList();
                //if (e.AccrualTimeJournal != null) {
                //    total = e.AccrualTimeJournal.Sum(t => (float?)t.Hours) ?? 0;
                //}
                total = ATJs.Sum(t => (float?)t.Hours) ?? 0;
                dr["TotalHours"] = total;
                dr["Percent"] = GetPercent(total, CapHours);
                dataTable.Rows.Add(dr);
            }
            dataTable.DefaultView.Sort = "CreateTime desc";

            ViewBag.userId = user.Id;
            ViewBag.Role = SystemVariates.LoginRoleName;
            return View(dataTable);
        }
        public ActionResult PTOHours() {
            if (Request["EmployeeId"] == null)
                return RedirectToAction("EmployeeList");
            Guid EmployeeId = new Guid(Request["EmployeeId"]);
            int Year = int.Parse(Request["Year"]);
            AppUser user = LoginUser;
            Employee e = db.Employee.Find(EmployeeId);
            DateTime startTime = TimeHelper.GetUTCTime(new DateTime(Year, 1, 1), Convert.ToDouble(user.TimeZone));
            DateTime endTime = TimeHelper.GetUTCTime(startTime.AddMonths(12), Convert.ToDouble(user.TimeZone));
            Dictionary<string, string> PTOTypeDic = EnumHelper.GetEnumItemValueDesc(typeof(PTOType));
            var atj = db.AccrualTimeJournal.Where(t => t.EmployeeId == EmployeeId && t.Date >= startTime && t.Date <= endTime).ToList();
            double TotalPTO = atj.Where(t => t.Type == 0).Sum(t => (double?)t.Hours) ?? 0;
            double TotalPV = atj.Where(t => t.Type == 1).Sum(t => (double?)t.Hours) ?? 0;
            ViewBag.TotalPTO = TotalPTO;
            ViewBag.CapPTO = GetStoragePTOHours(EmployeeId, Year, user, 0);
            ViewBag.TotalPV = TotalPV;
            ViewBag.CapPV = GetStoragePTOHours(EmployeeId, Year, user, 1);
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Id");
            dataTable.Columns.Add("EmployeeName");
            dataTable.Columns.Add("SSN");
            dataTable.Columns.Add("Date");
            dataTable.Columns.Add("Type");
            dataTable.Columns.Add("Hours");
            dataTable.Columns.Add("Memo");
            dataTable.Columns.Add("CreateDate");
            foreach (var r in atj)
            {
                DataRow dr = dataTable.NewRow();
                dr["Id"] = r.Id;
                dr["EmployeeName"] = r.EmployeeName;
                dr["SSN"] = r.SSN;
                dr["Date"] = TimeHelper.GetLocalTime(r.Date, Convert.ToDouble(user.TimeZone)).ToShortDateString().ToString();
                dr["Type"] = PTOTypeDic[r.Type.ToString()];
                dr["Hours"] = r.Hours;
                dr["Memo"] = r.Memo;
                dr["CreateDate"] = r.CreateDate;
                dataTable.Rows.Add(dr);
            }
            dataTable.DefaultView.Sort = "CreateDate desc";
            return View(dataTable);
        }
        private double GetStoragePTOHours(Guid EmployeeId,int Year,AppUser User,int Type) {
            double StoragHours = 0;
            DateTime startTime = TimeHelper.GetUTCTime(new DateTime(Year, 1, 1), Convert.ToDouble(User.TimeZone));
            DateTime endTime = TimeHelper.GetUTCTime(startTime.AddMonths(12), Convert.ToDouble(User.TimeZone));
            Employee e = db.Employee.Find(EmployeeId);
            float Rate = (Type == 0) ? e.PTOAccRate : e.VacAccRate;
            double CapHours = (Type == 0) ? e.PTOCapHours : e.VacCapHours;
            var ts = db.TimeSheet.Where(t => t.EmployeeId == EmployeeId && t.TimeSheetDate >= startTime && t.TimeSheetDate <= endTime).ToList();
            StoragHours = (ts.Sum(t => (double?)t.TotalWorkTime) ?? 0) * Rate;
            return Math.Round(((StoragHours <= CapHours) ? StoragHours : CapHours),2);
        }
        public ActionResult DeletePTO(Guid Id) {
            AccrualTimeJournal atj = db.AccrualTimeJournal.Find(Id);
            db.Entry(atj).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            return Json(new { code = 1, status = "success", message = "The PTO been successfully deleted!" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNewPTOHour(Guid EmployeeId) {
            AppUser user = LoginUser;
            Employee e = db.Employee.Find(EmployeeId);
            int Year = TimeHelper.GetLocalTime(DateTime.UtcNow, Convert.ToDouble(user.TimeZone)).Year;
            DateTime startTime = TimeHelper.GetUTCTime(new DateTime(Year, 1, 1), Convert.ToDouble(user.TimeZone));
            DateTime endTime = TimeHelper.GetUTCTime(startTime.AddMonths(12), Convert.ToDouble(user.TimeZone));
            double CapHours = 0;
            double PTOCapHours = GetStoragePTOHours(e.EmployeeId, Year, user, 0);
            //if (PTOCapHours >= VacCapHours) { CapHours = PTOCapHours; } else { CapHours = VacCapHours; }
            CapHours = PTOCapHours;
            
            float total = db.AccrualTimeJournal.Where(t => t.EmployeeId == e.EmployeeId && t.Date >= startTime && t.Date <= endTime).Sum(t => (float?)t.Hours) ?? 0;

            double Percent = GetPercent(total, CapHours);
            return Json(new { code = 1, status = "success", CapHours = CapHours, total = total, Percent = Percent }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEmployeePTO(Guid Id) {
            AppUser user = LoginUser;
            AccrualTimeJournal atj = db.AccrualTimeJournal.Find(Id);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", atj.Id);
            dic.Add("EmployeeId", atj.EmployeeId);
            dic.Add("Date", TimeHelper.GetLocalTime(atj.Date, Convert.ToDouble(user.TimeZone)).ToString("yyyy-MM-dd"));
            dic.Add("Type", atj.Type);
            dic.Add("Hours", atj.Hours);
            dic.Add("Usage", atj.Hours < 0 ? -1 : 1 );
            dic.Add("Memo", atj.Memo);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add()
        {
            AppUser user = LoginUser;
            Company com = db.Company.Find(user.CompanyId);
            var EmployeeList = db.Employee.Where<Employee>(e => e.CompanyId == user.CompanyId).ToList();
            var JobList = db.Job.Where<Job>(j => j.CompanyId == user.CompanyId).ToList();
            Dictionary<string, string> StaList = EnumHelper.GetEnumItemDesc(typeof(States));
            //Dictionary<string, object> StaList = EnumHelper.EnumListDic<States>("", "");
            ViewBag.StatesList = new SelectList(StaList, "key", "value");
            ViewBag.TimeZone = SelectHelper.TimeZoneToSelect(db);
            ViewData["EmployeeNum"] = EmployeeList.Count();
            ViewData["JobNum"] = JobList.Count();
            ViewBag.comTZ = com.TimeZone;
            ViewBag.comSta = com.State;
            ViewBag.comZip = com.Zip;
            
            ViewBag.UserRole = SystemVariates.LoginRoleName;
            var List102 = db.T102.Where<T102>(t => t.CompanyId == user.CompanyId).OrderBy(t => t.ItemId).ToList();
            ViewBag.List102 = List102;
            var List201 = db.T201.Where<T201>(t=>t.CompanyId== user.CompanyId && t.Type==1).OrderBy(t => t.Ord).ToList();
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
                var j = db.Job.Where(t => t.JobId == em.DefaultJob).SingleOrDefault();
                Emp.Job.Add(j);
            }
            AppUser user = UserManager.FindByEmail(em.Email);
            if (user == null)
            {
                try
                {
                    AppUser EmpUser = new AppUser { Email = em.Email, UserName = em.Email, CompanyId = LoginUser.CompanyId, TimeZone = Emp.TimeZone, PayRollUser = em.FName + " " + em.LName, PhoneNumber = em.Phone, LastLoginDate = DateTime.UtcNow };
                    UserManager.Create(EmpUser, "Pay123456");
                        Emp.AppUser = EmpUser;
                        db.Employee.Add(Emp);
                        if (!UserManager.IsInRole(EmpUser.Id, em.UserRole))
                        {
                            UserManager.AddToRole(EmpUser.Id, em.UserRole);
                        }

                    //jsonObj = string.Format("code = {0}, status = {1}, message = {2}", 1, "success", "success add");
                    jsonObj = new { code = 1, status = "success", message = "success add" ,userId = EmpUser.Id };
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
        public ActionResult addPTO(AccrualTimeJournal atj) {
            AppUser user = LoginUser;
            DateTime startTime = TimeHelper.GetUTCTime(new DateTime(atj.Date.Year, 1, 1), Convert.ToDouble(user.TimeZone));
            DateTime endTime = TimeHelper.GetUTCTime(startTime.AddMonths(12), Convert.ToDouble(user.TimeZone));
            Employee e = db.Employee.Where(t => t.EmployeeId == atj.EmployeeId).SingleOrDefault();
            float TotalThisYear = db.AccrualTimeJournal.Where(t => t.EmployeeId == atj.EmployeeId && t.Date >= startTime && t.Date <= endTime).Sum(t => (float?)t.Hours) ?? 0;
            atj.Id = Guid.NewGuid();
            atj.Date = TimeHelper.GetUTCTime(atj.Date, double.Parse(user.TimeZone));
            atj.EmployeeName = e.FName + " " + e.LName;
            atj.SSN = e.SSN;
            atj.CreateDate = DateTime.UtcNow;

            double CapPTO = GetStoragePTOHours(e.EmployeeId, atj.Date.Year, user, atj.Type);
            if ((TotalThisYear + atj.Hours) > CapPTO) {
                return Json(new { code = 3, status = "error", message = "Your remaining PTO time is only "+ (CapPTO - TotalThisYear) + " hours(" + atj.Date.Year + "), Please confirm and try again ! " }, JsonRequestBehavior.AllowGet);
            }
            db.AccrualTimeJournal.Add(atj);
            db.SaveChanges();
            return Json(new { code = 1, status = "success" , message = "success add!" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditPTO(Guid Id) {
            DateTime Date = DateTime.Parse(Request["Date"]);
            int Type = int.Parse(Request["Type"]);
            float Hours = float.Parse(Request["Hours"]);
            string Memo = Request["Hours"];
            AccrualTimeJournal atj = db.AccrualTimeJournal.Find(Id);
            atj.Date = Date;
            atj.Type = Type;
            atj.Hours = Hours;
            atj.Memo = Memo;
            db.Entry<AccrualTimeJournal>(atj).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(new { code = 1, status = "success", message = "success add!" }, JsonRequestBehavior.AllowGet);
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
                AppUser user = UserManager.FindById(Employee.AppUser.Id);
                if (UserManager.IsInRole(user.Id, "Admin") || Employee.UserRole == "Admin") {
                    return Json(new { code=2, message= "Admin cannot be deleted !" }, JsonRequestBehavior.AllowGet);
                }
                if (user != null)
                {
                    UserManager.Delete(user);
                }
                db.Entry<Employee>(Employee).State = System.Data.Entity.EntityState.Deleted;
                //删除Payroll list 记录
                var table = db.T105.Where(s => s.EmployeeId == Employee.EmployeeId).ToList();
                if (table.Count > 0)
                {
                    foreach (var a in table)
                    {
                        db.Entry<T105>(a).State = System.Data.Entity.EntityState.Deleted;
                        var Deletepath = Server.MapPath(ConfigurationManager.AppSettings["QRCodePath"]);
                        ZXingHelpers.FileDelte(Deletepath + a.Id + ".png");
                    }
                };
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
            Dictionary<string, string> StaList = EnumHelper.GetEnumItemDesc(typeof(States));
            ViewBag.StatesList = new SelectList(StaList, "key", "value");
            //Dictionary<string, string> StaList = EnumHelper.GetEnumItemDesc(typeof(States));
            //ViewBag.StatesList = new SelectList(StaList, "value", "key");
            ViewBag.TimeZone = SelectHelper.TimeZoneToSelect(db);
            ViewData["EmployeeNum"] = EmployeeList.Count();
            ViewData["JobNum"] = JobList.Count();
            ViewBag.UserRole = SystemVariates.LoginRoleName;
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
            Employee OldEmp = db.Employee.Where(t => t.EmployeeId == em.EmployeeId).SingleOrDefault();
            var OldEmp_DefaultJob = OldEmp.DefaultJob;
            var OldEmp_UserRole = OldEmp.UserRole;
            Employee NewEmp = db.Employee.Where(t => t.EmployeeId == em.EmployeeId).SingleOrDefault();
            NewEmp.LName = em.LName;
            NewEmp.FName = em.FName;
            NewEmp.MInit = em.MInit;
            NewEmp.SSN = em.SSN;
            NewEmp.F99 = em.F99;
            NewEmp.Address1 = em.Address1;
            NewEmp.Address2 = em.Address2;
            NewEmp.City = em.City;
            NewEmp.State = em.State;
            NewEmp.ZipCode = em.ZipCode;
            NewEmp.TimeZone = em.TimeZone;
            NewEmp.Phone = em.Phone;
            NewEmp.UserRole = em.UserRole;
            NewEmp.F100 = em.F100;
            NewEmp.F101 = em.F101;
            NewEmp.F102 = em.F102;
            NewEmp.F1002 = em.F1002;
            NewEmp.F106 = em.F106;
            NewEmp.F107 = em.F107;
            NewEmp.F108 = em.F108;
            NewEmp.F109 = em.F109;
            NewEmp.F110 = em.F110;
            NewEmp.F111 = em.F111;
            NewEmp.F112 = em.F112;
            NewEmp.F113 = em.F113;
            NewEmp.F114 = em.F114;
            NewEmp.F115 = em.F115;
            NewEmp.F116 = em.F116;
            NewEmp.F117 = em.F117;
            NewEmp.F118 = em.F118;
            NewEmp.F119 = em.F119;
            NewEmp.F120 = em.F120;
            NewEmp.F121 = em.F121;
            NewEmp.F122 = em.F122;
            NewEmp.F1231 = em.F1231;
            NewEmp.F1232 = em.F1232;
            NewEmp.F1233 = em.F1233;
            NewEmp.F1234 = em.F1234;
            NewEmp.F1235 = em.F1235;
            //NewEmp.F124 = em.F124;
            NewEmp.F125 = em.F125;
            NewEmp.IsW2StatutoryEmployee = em.IsW2StatutoryEmployee;
            NewEmp.IsW2RetirementPlan = em.IsW2RetirementPlan;
            NewEmp.DoesReceiveAdvanceEIC = em.DoesReceiveAdvanceEIC;
            NewEmp.F1236 = em.F1236;
            NewEmp.F1237 = em.F1237;
            NewEmp.SickRate = em.SickRate;
            NewEmp.VacationRate = em.VacationRate;
            NewEmp.is1099Employee = em.is1099Employee;
            NewEmp.PTOAcchours = em.PTOAcchours;
            NewEmp.VacAccHours = em.VacAccHours;
            NewEmp.printPTOStub = em.printPTOStub;
            NewEmp.PTOAccRate = em.PTOAccRate;
            NewEmp.VacAccRate = em.VacAccRate;
            NewEmp.PTOCapHours = em.PTOCapHours;
            NewEmp.VacCapHours = em.VacCapHours;
            NewEmp.isPayrollSetup = em.isPayrollSetup;
            NewEmp.DefaultJob = em.DefaultJob;
            NewEmp.isEmailComfirmed = em.isEmailComfirmed;
            NewEmp.SecurityStamp = em.SecurityStamp;

            string ErrorMessage = string.Empty;
            object jsonObj;
            string jsonStr = string.Empty;
            JavaScriptSerializer json = new JavaScriptSerializer();
            //if (em.DefaultJob != 0)
            //{
            //    var j = db.Job.Where(t => t.JobId == em.DefaultJob).ToList();
            //    NewEmp.Job = j;
            //}
            try
            {
                db.Entry(NewEmp).State = System.Data.Entity.EntityState.Modified;
                if (em.DefaultJob != 0 && OldEmp_DefaultJob!=em.DefaultJob)
                {
                    var j = db.Job.Where(t => t.JobId == em.DefaultJob).ToList();
                    NewEmp.Job = j;
                }
                db.SaveChanges();
                AppUser user = NewEmp.AppUser;
                user.PhoneNumber = em.Phone;
                user.TimeZone = em.TimeZone;
                user.PayRollUser = em.FName + " " + em.LName;
                UserManager.Update(user);

                jsonObj = new { code = 1, status = "success", message = "Employee Save Successful!" };

                if (OldEmp_UserRole != em.UserRole) {
                    UserManager.RemoveFromRole(user.Id, OldEmp_UserRole);
                    UserManager.AddToRoles(user.Id, em.UserRole);
                    if (NewEmp.AppUserId == User.Identity.GetUserId()) {
                        jsonObj = new { code = 2, status = "success", message = "Employee Save Successful!" };
                    }
                }
                
               
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
        public JsonResult StateTax(string StaId) {
            string StaCode = StaId;
            string XmlFileName = Server.MapPath(@"~/App_Data/DataTableXml.xml");
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
        //[HttpPost]
        //public async Task<ActionResult> SemdEmail(string Id) {
        //    AppUser user = UserManager.FindById(Id);
        //    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
        //    var callbackUrl = Url.Action("UserConfirm", "Account", new { userId = user.Id, code = code, Nt = TimeHelper.ConvertDateTimeInt(DateTime.UtcNow) }, protocol: Request.Url.Scheme);
        //    string EmailBody = "Thank you for creating an PayRoll account. You need to confirm your account and setting your password, you'll have access to PayRoll system.";
        //    string EmailLink = "Please confirm your account complete regist by clicking.<a href=\"" + callbackUrl + "\">Click it! &raquo;</a>";
        //    string strbody = ReplaceText(EmailBody, EmailLink);
        //    await UserManager.SendEmailAsync(user.Id, "Confirm Your Account  (" + user.PayRollUser + ")", strbody);
        //    return Json(new { code=1 , message = "success" }, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult FilingStatus(string StaCode) {
            string XmlFileName = Server.MapPath(@"~/App_Data/DataTableXml.xml");
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
            string XmlFileName = Server.MapPath(@"~/App_Data/DataTableXml.xml");
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
            em.F124 = 3;
            em.HourlyPay1 = "";
            em.HourlyPay2 = "";
            em.HourlyPay3 = "";
            em.HourlyPay4 = "";
            em.is1099Employee = 0;
            em.CompanyId = LoginUser.CompanyId;
            em.Job = new List<Job>();
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
        private double GetPercent(float TotalHours, double CapHours)
        {
            double P = 0;
            if (TotalHours > 0)
            {
                if (TotalHours > CapHours)
                {
                    P = 100;
                }
                else
                {
                    P = (TotalHours / CapHours) * 100;
                }
            }
            return Math.Round(P, 2);
        }
    }
}
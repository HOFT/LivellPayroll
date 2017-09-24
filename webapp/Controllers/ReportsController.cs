using LivellPayRoll.App_Helpers;
using LivellPayRoll.Enum;
using LivellPayRoll.Infrastructure;
using LivellPayRoll.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LivellPayRoll.Controllers
{
    [Authorize]
    [CustomAuthorize]
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult TimeCalendar()
        {
            AppUser user = LoginUser;
            ViewBag.TimeZone = user.TimeZone;
            return View();
        }
        public ActionResult Times(string TID) {
            AppUser user = LoginUser;
            Employee emp = user.Employee.SingleOrDefault();
            DateTime startMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);  //本月月初
            DateTime endMonth = startMonth.AddMonths(1); //本月月末
            DateTime startUtc = TimeHelper.GetUTCTime(startMonth, Convert.ToDouble(user.TimeZone));
            DateTime endUtc = TimeHelper.GetUTCTime(endMonth, Convert.ToDouble(user.TimeZone));
            string title = "";
            var times = db.TimeSheet.Where(t => t.CompanyId == user.CompanyId && t.EmployeeId == emp.EmployeeId).OrderByDescending(t => t.TimeSheetDate).ToList();
            if (TID == "T01")
            {
                 times = db.TimeSheet.Where(t => t.CompanyId == user.CompanyId && t.EmployeeId == emp.EmployeeId).OrderByDescending(t => t.TimeSheetDate).ToList();
                title = "Cumulative Time";
            }
            else if (TID == "T02")
            {
                times = db.TimeSheet.Where(t => t.CompanyId == user.CompanyId && t.EmployeeId == emp.EmployeeId && t.TimeSheetDate >= startUtc && t.TimeSheetDate <= endUtc).OrderByDescending(t => t.TimeSheetDate).ToList();
                title = "Month Times";
            }
            else if (TID == "T03")
            {
                DateTime lastStartUtc = startUtc.AddMonths(-1);
                DateTime lastEndUtc = endUtc.AddMonths(-1);
                times = db.TimeSheet.Where(t => t.CompanyId == user.CompanyId && t.EmployeeId == emp.EmployeeId && t.TimeSheetDate >= lastStartUtc && t.TimeSheetDate <= lastEndUtc).OrderByDescending(t => t.TimeSheetDate).ToList();
                title = "Last Month Times";
            }
            else if (TID == "T04") {
                 times = db.TimeSheet.Where(t => t.CompanyId == user.CompanyId && t.EmployeeId == emp.EmployeeId && t.Paid==false&&t.Status=="3").OrderByDescending(t => t.TimeSheetDate).ToList();
                title = "Locked Times";
            }
            ViewBag.Title = title;
            //Dictionary<string, string> TypeDic = EnumHelper.GetEnumItemValueDesc(typeof(TimeSheetType));
            ViewData["TypeDic"] = GetTimeSheetType(user.CompanyId);
            double cafeSales1 = 0;
            var list1 = db.TimeSheet.Where(t => t.CompanyId == user.CompanyId && t.EmployeeId == emp.EmployeeId);
            if (list1.Any())
            {
                cafeSales1 = list1.Sum(t => t.TotalWorkTime);
            }
            ViewBag.CUMULATIVE = cafeSales1;
            double cafeSales2 = 0;
            var list2 = db.TimeSheet.Where(t => t.CompanyId == user.CompanyId && t.EmployeeId == emp.EmployeeId && t.Paid==false);
            if (list2.Any())
            {
                cafeSales2 = list2.Sum(t => t.TotalWorkTime);
            }
            ViewBag.UNPAID = cafeSales2;
            double cafeSales3 = 0;
            var list3 = db.TimeSheet.Where(t => t.CompanyId == user.CompanyId && t.EmployeeId == emp.EmployeeId && t.Status=="3");
            if (list3.Any())
            {
                cafeSales3 = list3.Sum(t => t.TotalWorkTime);
            }
            ViewBag.UNPAID = cafeSales3;


            return View(times);
        }
        public JsonResult getTimeCalendar(DateTime CurrentDate)
        {
            DateTime startMonth = CurrentDate.AddDays(1 - CurrentDate.Day);  //本月月初  
            DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);  //本月月末  
            var emp = LoginUser.Employee.FirstOrDefault();
            if (emp != null)
            {
                var list = emp.TimeSheet.Select(e => new { Id = e.Id, StartDate = e.StartDate.ToString(), StopDate = e.StopDate.ToString(), TimeSheetDate = e.TimeSheetDate.ToString(), Note = e.Note, Paid = e.Paid, TotalWorkTime = e.TotalWorkTime, TimeSheetType = e.TimeSheetType, Locked = e.Status, JobName = e.Job.JobName });
                //var list = db.TimeSheet.Where(e => e.EmployeeId == emp.EmployeeId).Select(e => new { Id = e.Id, StartDate = e.StartDate.ToString(), StopDate = e.StopDate.ToString(), TimeSheetDate = e.TimeSheetDate.ToString(), Note = e.Note, Paid = e.Paid, TotalWorkTime = e.TotalWorkTime, TimeSheetType = e.TimeSheetType, Locked = e.Locked, JobName = e.Job.JobName }).ToString();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);


        }
        [Authorize]
        [CustomAuthorize]
        public ActionResult EmployerReport() {
            Dictionary<string, string> StaList = EnumHelper.GetEnumItemDesc(typeof(States));
            ViewBag.StatesList = new SelectList(StaList, "key", "value");
            return View();
        }
        public ActionResult RdlcView() {
            string aspx = "/Reports/RdlcView.aspx";
            DateTime LocalTime = TimeHelper.GetLocalTime(DateTime.UtcNow, double.Parse(LoginUser.TimeZone));
            string FromDate = (Request["dtpStartDate"] == "" || Request["dtpStartDate"] == null) ? DateTime.Now.ToString("yyyy-MM-dd") : Request["dtpStartDate"];
            string ToDate = (Request["dtpEndDate"] == "" || Request["dtpEndDate"] == null) ? DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") : Request["dtpEndDate"];
            string State = (Request["State"]=="" || Request["State"] == null) ?"All": Request["State"];
            string EmployeeId = (Request["EmployeeId"] == "" || Request["EmployeeId"] == null) ? "All" : Request["EmployeeId"];
            string Type= Request["Type"];
            string Year = (Request["Year"] == "" || Request["Year"] == null) ? LocalTime.Year.ToString(): Request["Year"];

            Session["dtpStartDate"] = FromDate;
            Session["dtpEndDate"] = ToDate;
            Session["State"] = State;
            Session["EmployeeId"] = EmployeeId;
            Session["Type"] = Type;
            Session["Year"] = Year;
            Session["CompanyId"] = LoginUser.CompanyId;
            Session["TimeZone"] = LoginUser.TimeZone;
            using (var sw = new StringWriter())
            {
                System.Web.HttpContext.Current.Server.Execute(aspx, sw, true);
                return Content(sw.ToString());
            }
        }
        public ActionResult EmployeeSummary()
        {
            return View();
        }
        public ActionResult Employeedetail() {
            return View();
        }
        public ActionResult EmployeePTO() {
            Dictionary<string, string> PTOTypeList = EnumHelper.GetEnumItemValueDesc(typeof(PTOType));
            ViewBag.PTOTypeList = new SelectList(PTOTypeList, "key", "value");
            return View();
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
        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
        private Dictionary<string, string> GetTimeSheetType(int CompanyId)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var list = db.T201.Where(t => t.CompanyId == CompanyId && t.Enabled == true && t.Type == 1).OrderBy(t => t.Ord).ToList();
            foreach (var r in list)
            {
                dic.Add(r.Ord.ToString(), r.Description);
            }
            return dic;
        }
    }
}
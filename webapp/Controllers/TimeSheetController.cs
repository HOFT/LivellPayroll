using LivellPayRoll.App_Helpers;
using LivellPayRoll.Enum;
using LivellPayRoll.Infrastructure;
using LivellPayRoll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace LivellPayRoll.Controllers
{
    [Authorize]
    public class TimeSheetController : Controller
    {
        // GET: TimeSheet
        public ActionResult SheetList()
        {
            var SheetList = db.TimeSheet.Where(t => t.CompanyId == LoginUser.CompanyId).ToList();
            ViewData.Model = SheetList;
            Dictionary<string, object> TypeList = EnumHelper.EnumListDic<TimeSheetType>("", "");
            ViewBag.TimeSheetType = new SelectList(TypeList, "value", "key");
            ViewBag.RoundTo = LoginUser.Company.RoundTo;
            ViewBag.TimeZone = LoginUser.TimeZone;
            Dictionary<string, string> TypeDic = EnumHelper.GetEnumItemValueDesc(typeof(TimeSheetType));
            ViewData["TypeDic"] = TypeDic;
            return View();
        }
        [HttpPost]
        public ActionResult AddTime()
        {
            Guid EmployeeId = new Guid(Request["EmployeeId"]);
            int JobId = int.Parse(Request["JobId"]);
            DateTime StartDate = Convert.ToDateTime(Request["StartDate"]);
            DateTime StopDate = Convert.ToDateTime(Request["StopDate"]);
            int Type = int.Parse(Request["TimeSheetType"]);
            Double TotalWorkTime = Math.Floor(((TimeHelper.ConvertDateTimeInt(StopDate)- TimeHelper.ConvertDateTimeInt(StartDate))/60)/ Convert.ToDouble(LoginUser.Company.RoundTo)) * Convert.ToDouble(LoginUser.Company.RoundTo);
            bool Paid = (Request["Paid"] == "true") ? true : false; 
            string Note = Request["Note"];

            Employee e = db.Employee.Find(EmployeeId);
            TimeSheet ts = new TimeSheet
            {
                
                Id = Guid.NewGuid(),
                StartDate = TimeHelper.GetUTCTime(StartDate, Convert.ToDouble(LoginUser.TimeZone)),
                StopDate = TimeHelper.GetUTCTime(StopDate, Convert.ToDouble(LoginUser.TimeZone)),
                Note = Note,
                Paid = Paid,
                TimeSheetDate = DateTime.UtcNow,
                TimeSheetType = Type,
                TotalWorkTime = TotalWorkTime,
                RegularPayrate = e.F100,
                OvertimePayrate = e.F101,
                Locked = false,
                EmployeeId = EmployeeId,
                JobId = JobId,
                CompanyId= LoginUser.CompanyId
            };
            if (Type == 1)
            {
                ts.RegulaWorkTime = TotalWorkTime;
            }
            else if(Type == 2){
                ts.SickTime = TotalWorkTime;
            }
            else if(Type == 3){
                ts.VacationTime = TotalWorkTime;
            };
            db.TimeSheet.Add(ts);
            db.SaveChanges();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("code", 1);
            dic.Add("status", "success");
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EditTime()
        {
            Guid id = new Guid(Request["Id"]);
            DateTime StartDate = Convert.ToDateTime(Request["StartDate"]);
            DateTime StopDate = Convert.ToDateTime(Request["StopDate"]);
            int Type = int.Parse(Request["TimeSheetType"]);
            Double TotalWorkTime = Math.Floor(((TimeHelper.ConvertDateTimeInt(StopDate) - TimeHelper.ConvertDateTimeInt(StartDate)) / 60) / Convert.ToDouble(LoginUser.Company.RoundTo)) * Convert.ToDouble(LoginUser.Company.RoundTo);
            bool Paid = (Request["Paid"] == "true") ? true : false;
            string Note = Request["Note"];
            TimeSheet ts = db.TimeSheet.Find(id);
            ts.StartDate = TimeHelper.GetUTCTime(StartDate, Convert.ToDouble(LoginUser.TimeZone));
            ts.StopDate = TimeHelper.GetUTCTime(StopDate, Convert.ToDouble(LoginUser.TimeZone));
            ts.Note = Note;
            ts.Paid = Paid;
            ts.TimeSheetType = Type;
            ts.TotalWorkTime = TotalWorkTime;
            if (Type == 1)
            {
                ts.RegulaWorkTime = TotalWorkTime;
            }
            else if (Type == 2)
            {
                ts.SickTime = TotalWorkTime;
            }
            else if (Type == 3)
            {
                ts.VacationTime = TotalWorkTime;
            };
            db.SaveChanges();
            return Json(new { code = "1", status = "success" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Locked()
        {
            Guid Id = new Guid(Request["Id"]);
            bool Locked = (Request["Locked"] == "true") ? true : false;
            TimeSheet ts = db.TimeSheet.Find(Id);
            ts.Locked = Locked;
            db.Entry(ts).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("code", 1);
            dic.Add("status", "success");
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult TimeDelete(Guid Id) {
            TimeSheet ts = db.TimeSheet.Find(Id);
            db.Entry<TimeSheet>(ts).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("code", 1);
            dic.Add("status", "success");
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SheetSetup()
        {
            Company c = db.Company.Find(LoginUser.CompanyId);
            return View(c);
        }
        [HttpPost]
        public ActionResult SheetSetup(Company c)
        {
            double DayRuleValue = Convert.ToDouble(Request["DayRuleValue"]);
            double DoubeRuleValue = Convert.ToDouble(Request["DoubeRuleValue"]);
            bool CaliforniaRule= (Request["CaliforniaRule"] == null)? false : true;
            bool allowedit = (Request["allowedit"] == null) ? false : true;
            Company com = db.Company.Find(LoginUser.CompanyId);
            com.WeekRule = false;
            com.WeekRuleValue = 40.00;
            com.DayRule = true;
            com.DayRuleValue = DayRuleValue;
            com.DoubeRule = true;
            com.DoubeRuleValue = DoubeRuleValue;
            com.CaliforniaRule = CaliforniaRule;
            com.allowedit = allowedit;
            db.Entry(com).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("SheetSetup");
        }
        public JsonResult GetEmployee()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            var Employees= db.Employee.Where<Employee>(e => e.CompanyId == LoginUser.CompanyId).ToList();
            foreach (var e in Employees)
            {
                items.Add(new SelectListItem() { Text=e.LName + " "+ e.FName+" ["+e.Email+"]" , Value=e.EmployeeId.ToString() });
            }
            
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetJob(string id)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(id))
            {
                var Jobs = db.Employee.Where<Employee>(e => e.CompanyId == LoginUser.CompanyId && e.EmployeeId == new Guid(id)).FirstOrDefault().Job;
                foreach (var j in Jobs)
                {
                    items.Add(new SelectListItem() { Text = j.JobName , Value = j.JobId.ToString() });
                }
                
            }
                return Json(items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult TimeDesc(Guid Id) {
            TS_Edit e = new TS_Edit();
            var ts = db.TimeSheet.Where<TimeSheet>(t => t.Id == Id).ToList().Single();
            e.Job = ts.Job.JobName;
            e.Employee = ts.Employee.FName+" "+ts.Employee.LName;
            e.TimeSheetType = ts.TimeSheetType;
            e.TotalWorkTime = ts.TotalWorkTime;
            e.Note = ts.Note;
            e.Paid = ts.Paid;
            e.StartDate = TimeHelper.ConvertDateTimeInt(TimeHelper.GetLocalTime(ts.StartDate, Convert.ToDouble(LoginUser.TimeZone)));
            e.StopDate = TimeHelper.ConvertDateTimeInt(TimeHelper.GetLocalTime(ts.StopDate, Convert.ToDouble(LoginUser.TimeZone)));
            return Json(e, JsonRequestBehavior.AllowGet);
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
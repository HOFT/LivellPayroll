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
using System.Data;

namespace LivellPayRoll.Controllers
{
    [Authorize]
    [CustomAuthorize]
    public class TimeSheetController : Controller
    {
        Dictionary<string, string> DicStatusu = EnumHelper.GetEnumItemValueDesc(typeof(TimeSheetStatus));
        // GET: TimeSheet
        public ActionResult SheetList()
        {
            AppUser user = LoginUser;
            ViewBag.TimeSheetType = SelectTimeSheetType(user.CompanyId);
            ViewBag.RoundTo = LoginUser.Company.RoundTo;
            Dictionary < string, string> DicTimeType = GetTimeSheetType(user.CompanyId);
            var SheetList = db.TimeSheet.Where(t => t.CompanyId == user.CompanyId).OrderByDescending(t => t.TimeSheetDate).ToList();
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Id");
            dataTable.Columns.Add("JobName");
            dataTable.Columns.Add("EmployeeName");
            dataTable.Columns.Add("EmployeeEmail");
            dataTable.Columns.Add("StartDate");
            dataTable.Columns.Add("StopDate");
            dataTable.Columns.Add("TimeSheetDate");
            dataTable.Columns.Add("TotalWorkTime");
            dataTable.Columns.Add("TimeSheetType");
            dataTable.Columns.Add("Paid");
            dataTable.Columns.Add("PaidLabel");
            dataTable.Columns.Add("Status");
            dataTable.Columns.Add("StatusLabel");

            string StatusLabel = "label-default";
            foreach (var r in SheetList) {
                DataRow dr = dataTable.NewRow();
                dr["Id"] = r.Id;
                dr["JobName"] = r.Job.JobName;
                dr["EmployeeName"] = r.Employee.FName + " " + r.Employee.LName;
                dr["EmployeeEmail"] = r.Employee.Email;
                dr["StartDate"] = TimeHelper.GetLocalTime(r.StartDate, Convert.ToDouble(user.TimeZone)).ToString();
                dr["StopDate"] = TimeHelper.GetLocalTime(r.StopDate, Convert.ToDouble(user.TimeZone)).ToString();
                dr["TimeSheetDate"] = TimeHelper.GetLocalTime(r.TimeSheetDate, Convert.ToDouble(user.TimeZone)).ToString();
                dr["TotalWorkTime"] = r.TotalWorkTime;
                dr["TimeSheetType"] = DicTimeType[r.TimeSheetType.ToString()];
                dr["Paid"] = (r.Paid)? "● Paid" : "● Unpaid";
                dr["PaidLabel"] = (r.Paid) ? "label-success" : "label-default";
                dr["Status"] = DicStatusu[r.Status];
                if (r.Status == "1") {
                    StatusLabel = "label-default";
                }else if(r.Status == "2"){
                    StatusLabel = "label-success";
                }else{
                    StatusLabel = "label-danger";
                };
                dr["StatusLabel"] = StatusLabel;
                dataTable.Rows.Add(dr);
            };
            return View(dataTable);
        }
        [HttpPost]
        public JsonResult Check() {
            Guid Id = new Guid(Request["Id"]);
            string Type = Request["CheckType"];
            TimeSheet ts = db.TimeSheet.Find(Id);
            ts.Status = Type;
            db.Entry<TimeSheet>(ts).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            string labelStr = "label-success";
            if (Type == "3") {
                labelStr = "label-danger";
            };
            string htmlStr = "<span class='center-block padding-5 label "+ labelStr + "'>"+ DicStatusu[Type] + "</span>";
            return Json(new { code = "1", status = "success", htmlStr = htmlStr }, JsonRequestBehavior.AllowGet);
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

            AppUser user = LoginUser;

            Company c = db.Company.Find(user.CompanyId);
            float DayRuleValue = c.DayRuleValue * 60;
            bool DayRule = c.DayRule;
            float DoubeRuleValue = c.DoubeRuleValue * 60;
            bool DoubeRule = c.DoubeRule;
            //Company c = db.Company.Where(t => t.CompanyId == user.CompanyId).SingleOrDefault();
            //if (c.DaylightSavingTime) {
            //    StartDate = StartDate.AddHours(1);
            //    StopDate = StopDate.AddHours(1);
            //}
            Employee e = db.Employee.Find(EmployeeId);
            TimeSheet ts = new TimeSheet
            {
                Id = Guid.NewGuid(),
                StartDate = TimeHelper.GetUTCTime(StartDate, Convert.ToDouble(user.TimeZone)),
                StopDate = TimeHelper.GetUTCTime(StopDate, Convert.ToDouble(user.TimeZone)),
                Note = Note,
                Paid = Paid,
                TimeSheetDate = DateTime.UtcNow,
                TimeSheetType = Type,
                TotalWorkTime = TotalWorkTime,
                RegulaWorkTime = 0,
                OverTimeWorkTime = 0,
                DoubleWorkTime = 0,
                RegularPayrate = e.F100,
                OvertimePayrate = e.F101,
                DoublePayrate = e.F1002,
                Status = "2",
                EmployeeId = EmployeeId,
                JobId = JobId,
                CompanyId = user.CompanyId
            };
            if (Type == 2)    //如果是普通时间
            {
                if (DoubeRule) {
                    ts.DoubleWorkTime = MathWorkTime(TotalWorkTime - DoubeRuleValue);
                }
                if (DayRule) {
                    ts.OverTimeWorkTime = MathWorkTime(TotalWorkTime - ts.DoubleWorkTime - DayRuleValue);
                }
                ts.RegulaWorkTime = MathWorkTime(TotalWorkTime - ts.DoubleWorkTime - ts.OverTimeWorkTime);
            }
            else if (Type == 3)    //如果类型是加班时间
            {            
                ts.OverTimeWorkTime = TotalWorkTime;
            }
            else if (Type == 4) {    //如果类型是双倍时间
                ts.DoubleWorkTime = TotalWorkTime;
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
            Company c = db.Company.Find(LoginUser.CompanyId);
            float DayRuleValue = c.DayRuleValue * 60;
            bool DayRule = c.DayRule;
            float DoubeRuleValue = c.DoubeRuleValue * 60;
            bool DoubeRule = c.DoubeRule;
            TimeSheet ts = db.TimeSheet.Find(id);
            ts.StartDate = TimeHelper.GetUTCTime(StartDate, Convert.ToDouble(LoginUser.TimeZone));
            ts.StopDate = TimeHelper.GetUTCTime(StopDate, Convert.ToDouble(LoginUser.TimeZone));
            ts.Status = "2";
            ts.Note = Note;
            ts.Paid = Paid;
            ts.TimeSheetType = Type;
            ts.TotalWorkTime = TotalWorkTime;
            ts.RegulaWorkTime = 0;
            ts.OverTimeWorkTime = 0;
            ts.DoubleWorkTime = 0;
            if (Type == 2)    //如果是普通时间
            {
                if (DoubeRule)
                {
                    ts.DoubleWorkTime = MathWorkTime(TotalWorkTime - DoubeRuleValue);
                }
                if (DayRule)
                {
                    ts.OverTimeWorkTime = MathWorkTime(TotalWorkTime - ts.DoubleWorkTime - DayRuleValue);
                }
                ts.RegulaWorkTime = MathWorkTime(TotalWorkTime - ts.DoubleWorkTime - ts.OverTimeWorkTime);
            }
            else if (Type == 3)    //如果类型是加班时间
            {
                ts.OverTimeWorkTime = TotalWorkTime;
            }
            else if (Type == 4)
            {    //如果类型是双倍时间
                ts.DoubleWorkTime = TotalWorkTime;
            };
            db.SaveChanges();
            return Json(new { code = "1", status = "success" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult rowDetail(Guid Id) {
            AppUser user = LoginUser;
            Dictionary<string, string> DicTimeType = GetTimeSheetType(user.CompanyId);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            TimeSheet ts = db.TimeSheet.Find(Id);
            dic.Add("JobName", ts.Job.JobName);
            dic.Add("EmployeeName", ts.Employee.FName+" "+ts.Employee.LName);
            dic.Add("EmployeeEmail", ts.Employee.Email);
            dic.Add("TimeSheetDate", TimeHelper.GetLocalTime(ts.TimeSheetDate, Convert.ToDouble(user.TimeZone)).ToString());
            float RegularAmount = (float)(ts.RegulaWorkTime / 60) * ts.RegularPayrate;
            float OvertimeAmount = (float)(ts.OverTimeWorkTime / 60) * ts.OvertimePayrate;
            float DoubletimeAmount = (float)(ts.DoubleWorkTime / 60) * ts.DoublePayrate;
            dic.Add("RegularAmount", RegularAmount.ToString("C"));
            dic.Add("OvertimeAmount", OvertimeAmount.ToString("C"));
            dic.Add("DoubletimeAmount", DoubletimeAmount.ToString("C"));
            dic.Add("Note", ts.Note);
            string TimeType = DicTimeType[ts.TimeSheetType.ToString()];
            if (ts.TimeSheetType == 2)
            {
                dic.Add("RegulaWorkTime", ts.RegulaWorkTime.ToString());
                dic.Add("OverTimeWorkTime", ts.OverTimeWorkTime.ToString());
                dic.Add("DoubleWorkTime", ts.DoubleWorkTime.ToString());
                TimeType = "Regulartime (minute)";
            }
            else {
                dic.Add("RegulaWorkTime", ts.TotalWorkTime.ToString());
                dic.Add("OverTimeWorkTime", "0");
                dic.Add("DoubleWorkTime", "0");
            };
            dic.Add("TimeType", TimeType);
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
            bool WeekRule = (Request["WeekRule"] == null) ? false : true;
            bool DayRule = (Request["DayRule"] == null) ? false : true;
            bool DoubeRule = (Request["DoubeRule"] == null) ? false : true;

            bool CaliforniaRule = (Request["CaliforniaRule"] == null) ? false : true;
            bool allowedit = (Request["allowedit"] == null) ? false : true;

            if (CaliforniaRule) {
                WeekRule = false;
                DayRule = false;
                DoubeRule = false;
            }
            if (!WeekRule) { c.WeekRuleValue = 0; }
            if (!DayRule) { c.DayRuleValue = 0; }
            if (!DoubeRule) { c.DoubeRuleValue = 0; }

            Company com = db.Company.Find(LoginUser.CompanyId);
            com.WeekRule = WeekRule;
            com.WeekRuleValue = c.WeekRuleValue;
            com.DayRule = DayRule;
            com.DayRuleValue = c.DayRuleValue;
            com.DoubeRule = DoubeRule;
            com.DoubeRuleValue = c.DoubeRuleValue;
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
        private Dictionary<string, string> GetTimeSheetType(int CompanyId) {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var list = db.T201.Where(t => t.CompanyId == CompanyId && t.Enabled == true && t.Type == 1).OrderBy(t=>t.Ord).ToList();
            foreach (var r in list) {
                dic.Add(r.Ord.ToString(), r.Description);
            }
            return dic;
        }
        private SelectList SelectTimeSheetType(int CompanyId) {
            List<SelectListItem> List = new List<SelectListItem>();
            var list = db.T201.Where(t => t.CompanyId == CompanyId && t.Enabled == true && t.Type == 1 && t.Ord !=1 ).OrderBy(t => t.Ord).ToList();
            foreach (var t in list)
            {
                List.Add(new SelectListItem() { Text = t.Description, Value = t.Ord.ToString() });
            }
            return new SelectList(List, "Value", "Text");
        }
        private double MathWorkTime(double _v) {
            double value = (_v >= 0) ? _v : 0;
            return value;
        }
    }
}
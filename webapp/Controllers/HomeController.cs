#region Using
using LivellPayRoll.App_Helpers;
using LivellPayRoll.Infrastructure;
using LivellPayRoll.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

#endregion

namespace LivellPayRoll.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: home/index
        public ActionResult Index()
        {
            var log = LoginUser.TimeSheetLog.Where(t => t.Status == false).LastOrDefault();
            if (log != null)
            {
                ViewBag.StartDate = log.StartDate;
            }
            else {
                ViewBag.StartDate = 0;
            };
            Dictionary<string, object> JobList = DicJobs();
            ViewBag.JobList = new SelectList(JobList, "value", "key");
            ViewBag.RoundTo = LoginUser.Company.RoundTo;
            ViewBag.TimeZone = LoginUser.TimeZone; 
            return View(log);
        }
        public JsonResult getTimeCalendar(DateTime CurrentDate) {
            DateTime startMonth = CurrentDate.AddDays(1 - CurrentDate.Day);  //本月月初  
            DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);  //本月月末  
            var emp = LoginUser.Employee.FirstOrDefault();
            if (emp != null) {
                var list = emp.TimeSheet.Select(e => new { Id = e.Id, StartDate = e.StartDate.ToString(), StopDate = e.StopDate.ToString(), TimeSheetDate = e.TimeSheetDate.ToString(), Note = e.Note, Paid = e.Paid, TotalWorkTime = e.TotalWorkTime, TimeSheetType = e.TimeSheetType, Locked = e.Locked, JobName = e.Job.JobName });
                //var list = db.TimeSheet.Where(e => e.EmployeeId == emp.EmployeeId).Select(e => new { Id = e.Id, StartDate = e.StartDate.ToString(), StopDate = e.StopDate.ToString(), TimeSheetDate = e.TimeSheetDate.ToString(), Note = e.Note, Paid = e.Paid, TotalWorkTime = e.TotalWorkTime, TimeSheetType = e.TimeSheetType, Locked = e.Locked, JobName = e.Job.JobName }).ToString();
                
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
            

        }
        public JsonResult TimeIn(TimeSheetLog TSL) {
            var log = LoginUser.TimeSheetLog.Where(t => t.Status == false).LastOrDefault();
            if (log != null)
            {
                return Json(new { code = "2", message = "You have a unfinished work!" }, JsonRequestBehavior.AllowGet);
            }
            TSL.Id= Guid.NewGuid();
            TSL.StartDate =DateTime.UtcNow;
            TSL.StopDate = DateTime.UtcNow;
            TSL.Status = false;
            TSL.AppUserId = LoginUser.Id;
            db.TimeSheetLog.Add(TSL);
            db.SaveChanges();
            //DateTime LocalTime = TimeHelper.GetLocalTime(TSL.StartDate, SystemVariates.TimeZone);
            return Json(new { code = "1", message = "success!" , StartDate = TSL.StartDate.ToString(),JobId=TSL.JobId,Note=TSL.Note }, JsonRequestBehavior.AllowGet);
            
        }
        public JsonResult TimeReset(TimeSheetLog TSL)
        {
            var log = LoginUser.TimeSheetLog.Where(t => t.Status == false).LastOrDefault();
            db.Entry<TimeSheetLog>(log).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return Json(new { code = "1", message = "success!" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult TimeOut(TimeSheetLog TSL)
        {
            var log = LoginUser.TimeSheetLog.Where(t => t.Status == false).LastOrDefault();
            var e = LoginUser.Employee.SingleOrDefault();
            Double TotalWorkTime = Math.Floor(((TimeHelper.ConvertDateTimeInt(DateTime.UtcNow) - TimeHelper.ConvertDateTimeInt(log.StartDate)) / 60) / Convert.ToDouble(LoginUser.Company.RoundTo)) * Convert.ToDouble(LoginUser.Company.RoundTo);
            TimeSheet ts = new TimeSheet
            {
                Id = Guid.NewGuid(),
                StartDate = log.StartDate,
                StopDate = DateTime.UtcNow,
                Note = log.Note,
                Paid = false,
                TimeSheetDate = DateTime.UtcNow,
                TimeSheetType = 1,
                TotalWorkTime = TotalWorkTime,
                RegularPayrate = e.F100,
                OvertimePayrate = e.F101,
                Locked = false,
                EmployeeId = e.EmployeeId,
                JobId = log.JobId,
                CompanyId = LoginUser.CompanyId
            };
            log.StopDate = DateTime.UtcNow;
            log.Status = true;
            db.Entry<TimeSheetLog>(log).State = System.Data.Entity.EntityState.Modified;
            db.TimeSheet.Add(ts);
            db.SaveChanges();
            return Json(new { code = "1", message = "success!" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult TimeCard(Guid Id)
        {
            TimeSheet ts=db.TimeSheet.Find(Id);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("JobName", ts.Job.JobName);
            dic.Add("Date", ts.TimeSheetDate.ToShortDateString());
            dic.Add("Paid", ts.Paid);
            dic.Add("Locked", ts.Locked);
            dic.Add("StartDate", ts.StartDate.ToString());
            dic.Add("StopDate", ts.StopDate.ToString());
            dic.Add("TotalWorkTime", ts.TotalWorkTime);
            dic.Add("Note", ts.Note);
            dic.Add("TimeSheetType", ts.TimeSheetType);
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        public JsonResult TimeDelete(Guid Id)
        {
            var ts = db.TimeSheet.Find(Id);
            db.Entry<TimeSheet>(ts).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return Json(new { code = "1", message = "success!" }, JsonRequestBehavior.AllowGet);
        }
        private Dictionary<string, object> DicJobs()
        {
            Dictionary<string, object> dicjob = new Dictionary<string, object>();
            AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(db));
            var emp = LoginUser.Employee.FirstOrDefault();
            if (emp != null) {
                foreach (var job in emp.Job)
                {
                    dicjob.Add(job.JobName, job.JobId);
                }
            }
            
            return dicjob;
        }
        private AppIdentityDbContext db
        {
            get
            {
                return HttpContext.GetOwinContext().Get<AppIdentityDbContext>();
            }
        }
        private  AppUser LoginUser
        {
            get
            {
                //AppUser user = HttpContext.User;
                AppUser user =  UserManager.FindByName(HttpContext.User.Identity.Name);
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
    }
}
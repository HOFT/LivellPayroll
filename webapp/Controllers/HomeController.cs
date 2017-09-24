#region Using
using LivellPayRoll.App_Helpers;
using LivellPayRoll.Enum;
using LivellPayRoll.Infrastructure;
using LivellPayRoll.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

#endregion

namespace LivellPayRoll.Controllers
{
    [Authorize]
    [CustomAuthorize]
    public class HomeController : Controller
    {
        // GET: home/index
        public ActionResult Index()
        {
            AppUser user = LoginUser;
            DateTime startMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);  //本月月初
            DateTime endMonth = startMonth.AddMonths(1); //本月月末
            DateTime startUtc = TimeHelper.GetUTCTime(startMonth, Convert.ToDouble(user.TimeZone));
            DateTime endUtc = TimeHelper.GetUTCTime(endMonth, Convert.ToDouble(user.TimeZone));
            DateTime lastStartUtc = startUtc.AddMonths(-1);
            DateTime lastEndUtc = endUtc.AddMonths(-1);
            ViewBag.RoundTo = user.Company.RoundTo;
            ViewBag.TimeZone = user.TimeZone;
            Dictionary<string, string> DicStatus = EnumHelper.GetEnumItemValueDesc(typeof(TimeSheetStatus));
            ViewBag.DicStatus = DicStatus;
            bool DaylightSavingTime = false;
            if (LocalIsDaylightSavingTime(DateTime.Now, user.TimeZone) && user.Company.DaylightSavingTime) {
                 DaylightSavingTime = true;
            }
            ViewBag.DaylightSavingTime = DaylightSavingTime;

            //管理层数据模块
            var TimeList = db.TimeSheet.Where(t => t.CompanyId == user.CompanyId && t.TimeSheetDate >= startUtc && t.TimeSheetDate <= endUtc).ToList();
            ViewBag.NewTimes = TimeList.Count;
            var EmployeeList = db.Employee.Where(t => t.CompanyId == user.CompanyId).ToList();
            ViewBag.EmpNum = EmployeeList.Count;
            var ActiveList = db.Employee.Where(t => t.CompanyId == user.CompanyId && t.F124 == 0).ToList();
            ViewBag.Active = ActiveList.Count;
            var ProjList = db.Job.Where(t => t.CompanyId == user.CompanyId).ToList();
            ViewBag.ProjNum = ProjList.Count;

            var ts = db.TimeSheet.Where(t => t.CompanyId == user.CompanyId).ToList();
            var EmpCount = db.Employee.Where(t => t.CompanyId == user.CompanyId).ToList().Count;
            Dictionary<string, double> dicJobTime = new Dictionary<string, double>();
            Dictionary<string, double> dicYearTime = new Dictionary<string, double>();
            for (int i = 1; i <= 12; i++)
            {
                DateTime thisStartMonth = new DateTime(DateTime.Now.Year, i, 1);  //本月月初
                DateTime thisEndMonth = thisStartMonth.AddMonths(1); //本月月末
                DateTime thisStartUtc = TimeHelper.GetUTCTime(thisStartMonth, Convert.ToDouble(user.TimeZone));
                DateTime thisEndUtc = TimeHelper.GetUTCTime(thisEndMonth, Convert.ToDouble(user.TimeZone));
                double monthTotal = ts.Where(t => t.TimeSheetDate >= thisStartUtc && t.TimeSheetDate <= thisEndUtc).Sum(t => (double?)t.TotalWorkTime) ?? 0;
                dicJobTime.Add("M" + i, monthTotal / EmpCount);
                dicYearTime.Add("M" + i, monthTotal);
            }
            ViewBag.avgMonthTime = dicJobTime;
            ViewBag.dicYearTime = dicYearTime;
            DateTime yearStartMonth = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime yearEndMonth = yearStartMonth.AddMonths(12);
            DateTime yearStartUtc = TimeHelper.GetUTCTime(yearStartMonth, Convert.ToDouble(user.TimeZone));
            DateTime yearEndUtc = TimeHelper.GetUTCTime(yearEndMonth, Convert.ToDouble(user.TimeZone));
            double yearTotal = ts.Where(t => t.TimeSheetDate >= yearStartUtc && t.TimeSheetDate <= yearEndUtc).Sum(t => (double?)t.TotalWorkTime) ?? 0;
            ViewBag.yearTotal = yearTotal;
            Dictionary<string, double> TimeSheetType = new Dictionary<string, double>();
            double RegularWorkTime = ts.Where(t => t.TimeSheetDate >= yearStartUtc && t.TimeSheetDate <= yearEndUtc).Sum(t => (double?)t.RegulaWorkTime) ?? 0;
            double OverTimeWorkTime = ts.Where(t => t.TimeSheetDate >= yearStartUtc && t.TimeSheetDate <= yearEndUtc).Sum(t => (double?)t.OverTimeWorkTime) ?? 0;
            double DoubleWorkTime = ts.Where(t => t.TimeSheetDate >= yearStartUtc && t.TimeSheetDate <= yearEndUtc).Sum(t => (double?)t.DoubleWorkTime) ?? 0;
            double allTotal = ts.Where(t => t.TimeSheetDate >= yearStartUtc && t.TimeSheetDate <= yearEndUtc).Sum(t => (double?)t.TotalWorkTime) ?? 0;
            double OtherTime = allTotal - RegularWorkTime - OverTimeWorkTime - DoubleWorkTime;
            TimeSheetType.Add("RegularWorkTime", RegularWorkTime);
            TimeSheetType.Add("OverTimeWorkTime", OverTimeWorkTime);
            TimeSheetType.Add("DoubleWorkTime", DoubleWorkTime);
            TimeSheetType.Add("OtherTime", OtherTime);
            ViewBag.TimeSheetType = TimeSheetType;
            //管理层数据模块End

            //打卡数据
            var log = LoginUser.TimeSheetLog.Where(t => t.Status == false).LastOrDefault();
            if (log != null)
            {
                ViewBag.StartDate = log.StartDate;
            }
            else
            {
                ViewBag.StartDate = 0;
            };
            Dictionary<string, object> JobList = DicJobs();
            ViewBag.JobList = new SelectList(JobList, "value", "key");
            ViewBag.TimeList = "";
            //数据部分
            Employee emp = user.Employee.SingleOrDefault();
            var Times = db.TimeSheet.Where(t => t.CompanyId == user.CompanyId && t.EmployeeId == emp.EmployeeId).OrderByDescending(t => t.TimeSheetDate).ToList();
            ViewBag.TimeList = Times;
            if (emp.DefaultJob != 0)
            {
                ViewBag.defaultjob = emp.DefaultJob;
            }

            if (UserManager.IsInRole(user.Id, "Employee"))
            {
                double cafeSales1 = 0;
                double cafeSales2 = 0;
                double cafeSales3 = 0;
                var list1 = db.TimeSheet.Where(t => t.CompanyId == user.CompanyId && t.EmployeeId == emp.EmployeeId);
                if (list1.Any())
                {
                    cafeSales1 = list1.Sum(t => t.TotalWorkTime);
                }
                ViewBag.T01 = cafeSales1;

                var list2 = db.TimeSheet.Where(t => t.CompanyId == user.CompanyId && t.EmployeeId == emp.EmployeeId && t.TimeSheetDate >= startUtc && t.TimeSheetDate <= endUtc);
                if (list2.Any())
                {
                    cafeSales2 = list2.Sum(t => t.TotalWorkTime);
                }
                ViewBag.T02 = cafeSales2;

                var list3 = db.TimeSheet.Where(t => t.CompanyId == user.CompanyId && t.EmployeeId == emp.EmployeeId && t.TimeSheetDate >= lastStartUtc && t.TimeSheetDate <= lastEndUtc);
                if (list3.Any())
                {
                    cafeSales3 = list3.Sum(t => t.TotalWorkTime);
                }
                ViewBag.T03 = cafeSales3;

                var list4 = db.TimeSheet.Where(t => t.CompanyId == user.CompanyId && t.EmployeeId == emp.EmployeeId && t.Paid == false && t.Status == "1").ToList();
                ViewBag.T04 = list4.Count;

            }
                ////var JobList = db.Job.Where(t => t.CompanyId == user.CompanyId).GroupBy(t => new { t.JobId, t.JobName }).Select(t => new { ProjId = t.Key.JobId, ProjName = t.Key.JobName, TimeTotal = t.Sum(u => u.TimeSheet.Sum(s => s.TotalWorkTime)) }).ToList();
                //var JobList = db.Job.Where(t => t.CompanyId == user.CompanyId).GroupBy(t => new { t.JobId, t.JobName }).Select(t => new {ProjId=t.Key.JobId, ProjName = t.Key.JobName }).ToList();
                //DataTable dataTable = new DataTable();
                //dataTable.Columns.Add("JobName");
                //dataTable.Columns.Add("PCount");
                //dataTable.Columns.Add("ThisTime");
                //dataTable.Columns.Add("TimeTotal");
                //dataTable.Columns.Add("PCT");
                //dataTable.Columns.Add("BarColor");
                //foreach (var job in JobList) {
                //    Job j = db.Job.Where(t => t.JobId == job.ProjId).SingleOrDefault();
                //    double ThisTime = db.TimeSheet.Where(t => t.CompanyId == user.CompanyId && t.JobId == job.ProjId && t.TimeSheetDate > startUtc && t.TimeSheetDate < endUtc).Sum(t=> (double?)t.TotalWorkTime) ?? 0;
                //    double percent = ThisTime / Convert.ToDouble(5000);
                //    double TimeTotal = db.TimeSheet.Where(t => t.CompanyId == user.CompanyId && t.JobId == job.ProjId).Sum(t => (double?)t.TotalWorkTime) ?? 0;
                //    string PCT = percent.ToString("p");//可以到百分数
                //    DataRow dr = dataTable.NewRow();
                //    dr["JobName"] = job.ProjName;
                //    dr["PCount"] = j.Employee.Count;
                //    dr["ThisTime"] = ThisTime;
                //    dr["TimeTotal"] = TimeTotal;
                //    dr["PCT"] = PCT;
                //    if (percent <= 0.2)
                //    {
                //        dr["BarColor"] = "txt-color-blueLight";
                //    }
                //    else if (percent <= 0.4)
                //    {
                //        dr["BarColor"] = "txt-color-blue";
                //    }
                //    else if (percent <= 0.6)
                //    {
                //        dr["BarColor"] = "txt-color-green";
                //    }
                //    else if (percent <= 0.8)
                //    {
                //        dr["BarColor"] = "txt-color-orange";
                //    }
                //    else {
                //        dr["BarColor"] = "txt-color-red";

                //    }
                //        dataTable.Rows.Add(dr);
                //}
                //dataTable.DefaultView.Sort = "ThisTime desc";
                //dataTable = GetPagedTable(dataTable, 1, 4);
                //ViewBag.JobList = dataTable.Rows;

            return View(log);
        }
        public JsonResult GetJobList(int PageIndex, int PageSize) {
            //int PageSize = 4;
            decimal PageNum = 0;
            AppUser user = LoginUser;
            DateTime startMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);  //本月月初
            DateTime endMonth = startMonth.AddMonths(1); //本月月末
            DateTime startUtc = TimeHelper.GetUTCTime(startMonth, Convert.ToDouble(user.TimeZone));
            DateTime endUtc = TimeHelper.GetUTCTime(endMonth, Convert.ToDouble(user.TimeZone));

            var JobList = db.Job.Where(t => t.CompanyId == user.CompanyId).GroupBy(t => new { t.JobId, t.JobName }).Select(t => new { ProjId = t.Key.JobId, ProjName = t.Key.JobName }).ToList();
            PageNum = Math.Ceiling(Convert.ToDecimal((Convert.ToDouble(JobList.Count) / Convert.ToDouble(PageSize))));
            List<DataTableJobItemModel> list = new List<DataTableJobItemModel>();
            foreach (var job in JobList)
            {
                Job j = db.Job.Where(t => t.JobId == job.ProjId).SingleOrDefault();
                double ThisTime = db.TimeSheet.Where(t => t.CompanyId == user.CompanyId && t.JobId == job.ProjId && t.TimeSheetDate >= startUtc && t.TimeSheetDate <= endUtc).Sum(t => (double?)t.TotalWorkTime)??0;
                double percent = ThisTime / Convert.ToDouble(5000);
                double TimeTotal = db.TimeSheet.Where(t => t.CompanyId == user.CompanyId && t.JobId == job.ProjId).Sum(t => (double?)t.TotalWorkTime)??0;
                string PCT = percent.ToString("p");//可以到百分数
                string BarColor;
                if (percent <= 0.2)
                {
                    BarColor = "txt-color-blueLight";
                }
                else if (percent <= 0.4)
                {
                    BarColor = "txt-color-blue";
                }
                else if (percent <= 0.6)
                {
                    BarColor = "txt-color-green";
                }
                else if (percent <= 0.8)
                {
                    BarColor = "txt-color-orange";
                }
                else
                {
                    BarColor = "txt-color-red";

                }

                DataTableJobItemModel model = new DataTableJobItemModel() {
                    JobId = job.ProjId,
                    JobName = job.ProjName,
                    PCount = j.Employee.Count,
                    ThisTime = ThisTime,
                    TimeTotal = TimeTotal,
                    PCT = PCT,
                    BarColor = BarColor
                };
                list.Add(model);
            }
            list = list.OrderByDescending(t => t.ThisTime).Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            return Json(new { PageIndex = PageIndex, PageNum = PageNum, dataString = list }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetJobChart(int JobId)
        {
            AppUser user = LoginUser;
            var ts = db.TimeSheet.Where(t => t.JobId == JobId && t.CompanyId == user.CompanyId).ToList();
            Dictionary<string, double> dic = new Dictionary<string, double>();
            for (int i = 1; i <= 12; i++) {
                DateTime startMonth = new DateTime(DateTime.Now.Year, i, 1);  //本月月初
                DateTime endMonth = startMonth.AddMonths(1); //本月月末
                DateTime startUtc = TimeHelper.GetUTCTime(startMonth, Convert.ToDouble(user.TimeZone));
                DateTime endUtc = TimeHelper.GetUTCTime(endMonth, Convert.ToDouble(user.TimeZone));
                double monthTotal = ts.Where(t => t.TimeSheetDate >= startUtc && t.TimeSheetDate <= endUtc).Sum(t => (double?)t.TotalWorkTime) ?? 0;
                dic.Add("month"+i, monthTotal);
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getTimeCalendar(DateTime CurrentDate) {
            DateTime startMonth = CurrentDate.AddDays(1 - CurrentDate.Day);  //本月月初  
            DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);  //本月月末  
            var emp = LoginUser.Employee.FirstOrDefault();
            if (emp != null) {
                var list = emp.TimeSheet.Select(e => new { Id = e.Id, StartDate = e.StartDate.ToString(), StopDate = e.StopDate.ToString(), TimeSheetDate = e.TimeSheetDate.ToString(), Note = e.Note, Paid = e.Paid, TotalWorkTime = e.TotalWorkTime, TimeSheetType = e.TimeSheetType, Status = e.Status, JobName = e.Job.JobName });
                //var list = db.TimeSheet.Where(e => e.EmployeeId == emp.EmployeeId).Select(e => new { Id = e.Id, StartDate = e.StartDate.ToString(), StopDate = e.StopDate.ToString(), TimeSheetDate = e.TimeSheetDate.ToString(), Note = e.Note, Paid = e.Paid, TotalWorkTime = e.TotalWorkTime, TimeSheetType = e.TimeSheetType, Locked = e.Locked, JobName = e.Job.JobName }).ToString();
                
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
            

        }
        [HttpPost]
        public JsonResult TimeIn() {
            AppUser user = LoginUser;
            int JobId = int.Parse(Request["JobId"]);
            string Note = Request["Note"];
            bool isDayLightTime = LocalIsDaylightSavingTime(DateTime.Now, user.TimeZone);
            DateTime StartDate = DateTime.UtcNow;
            var log = user.TimeSheetLog.Where(t => t.Status == false).LastOrDefault();
            if (log != null)
            {
                return Json(new { code = "2", message = "You have a unfinished work!" }, JsonRequestBehavior.AllowGet);
            }
            if (user.Company.DaylightSavingTime && isDayLightTime)
            {
                StartDate = StartDate.AddHours(1);
            }
            TimeSheetLog TSL = new TimeSheetLog
            {
                Id = Guid.NewGuid(),
                JobId = JobId,
                Note = Note,
                isDayLightTime = isDayLightTime,
                AppUserId = user.Id,
                Status = false,
                StartDate = StartDate,
                StopDate = StartDate
            };
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
            AppUser user = LoginUser;
            Company c = db.Company.Find(user.CompanyId);
            float DayRuleValue = c.DayRuleValue * 60;
            bool DayRule = c.DayRule;
            float DoubeRuleValue = c.DoubeRuleValue * 60;
            bool DoubeRule = c.DoubeRule;

            var log = user.TimeSheetLog.Where(t => t.Status == false).LastOrDefault();
            var utcNowTime = DateTime.UtcNow;
            if (user.Company.DaylightSavingTime && log.isDayLightTime) {
                utcNowTime = utcNowTime.AddHours(1);
            }
            var e = user.Employee.SingleOrDefault();
            Double TotalWorkTime = Math.Floor(((TimeHelper.ConvertDateTimeInt(utcNowTime) - TimeHelper.ConvertDateTimeInt(log.StartDate)) / 60) / Convert.ToDouble(user.Company.RoundTo)) * Convert.ToDouble(user.Company.RoundTo);
            TimeSheet ts = new TimeSheet
            {
                Id = Guid.NewGuid(),
                StartDate = log.StartDate,
                StopDate = utcNowTime,
                Note = log.Note,
                Paid = false,
                TimeSheetDate = utcNowTime,
                TimeSheetType = 2,
                TotalWorkTime = TotalWorkTime,
                RegulaWorkTime = 0,
                OverTimeWorkTime = 0,
                DoubleWorkTime = 0,
                RegularPayrate = e.F100,
                OvertimePayrate = e.F101,
                DoublePayrate = e.F1002,
                Status = "1",
                EmployeeId = e.EmployeeId,
                JobId = log.JobId,
                CompanyId = user.CompanyId
            };
            if (DoubeRule)
            {
                ts.DoubleWorkTime = MathWorkTime(TotalWorkTime - DoubeRuleValue);
            }
            if (DayRule)
            {
                ts.OverTimeWorkTime = MathWorkTime(TotalWorkTime - ts.DoubleWorkTime - DayRuleValue);
            }
            ts.RegulaWorkTime = MathWorkTime(TotalWorkTime - ts.DoubleWorkTime - ts.OverTimeWorkTime);

            log.StopDate = utcNowTime;
            log.Status = true;
            db.Entry<TimeSheetLog>(log).State = System.Data.Entity.EntityState.Modified;
            db.TimeSheet.Add(ts);
            db.SaveChanges();
            return Json(new { code = "1", message = "success!" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult TimeCard(Guid Id)
        {
            Dictionary<string, string> DicStatusu = EnumHelper.GetEnumItemValueDesc(typeof(TimeSheetStatus));
            Dictionary<string, string> DicTimeType = GetTimeSheetType(LoginUser.CompanyId);
            TimeSheet ts=db.TimeSheet.Find(Id);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("JobName", ts.Job.JobName);
            dic.Add("Date", ts.TimeSheetDate.ToShortDateString());
            dic.Add("Paid", ts.Paid);
            dic.Add("Status", DicStatusu[ts.Status]);
            dic.Add("StartDate", ts.StartDate.ToString());
            dic.Add("StopDate", ts.StopDate.ToString());
            dic.Add("TotalWorkTime", ts.TotalWorkTime);
            dic.Add("Note", ts.Note);

            string TimeType = DicTimeType[ts.TimeSheetType.ToString()];
            if (ts.TimeSheetType == 2)
            {
                dic.Add("RegulaWorkTime", ts.RegulaWorkTime.ToString());
                dic.Add("OverTimeWorkTime", ts.OverTimeWorkTime.ToString());
                dic.Add("DoubleWorkTime", ts.DoubleWorkTime.ToString());
                TimeType = "Regulartime (minute)";
            }
            else
            {
                dic.Add("RegulaWorkTime", ts.TotalWorkTime.ToString());
                dic.Add("OverTimeWorkTime", "0");
                dic.Add("DoubleWorkTime", "0");
            };
            dic.Add("TimeType", TimeType);
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
        /// <summary>DataTable序列化
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string SerializeDataTable(DataTable dt)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)//每一行信息，新建一个Dictionary<string,object>,将该行的每列信息加入到字典
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc].ToString());
                }
                list.Add(result);
            }
            return serializer.Serialize(list);//调用Serializer方法 
        }
        private DataTable GetPagedTable(DataTable dt, int PageIndex, int PageSize)//PageIndex表示第几页，PageSize表示每页的记录数
        {
            if (PageIndex == 0)
                return dt;//0页代表每页数据，直接返回

            DataTable newdt = dt.Copy();
            newdt.Clear();//copy dt的框架

            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;

            if (rowbegin >= dt.Rows.Count)
                return newdt;//源数据记录数小于等于要显示的记录，直接返回dt

            if (rowend > dt.Rows.Count)
                rowend = dt.Rows.Count;
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }
            return newdt;
        }
        private bool LocalIsDaylightSavingTime(DateTime LocalTime,string LocalTimeZome) {
            bool result = false;
            TimeZoneInfo local = TimeZoneInfo.Local;
            DateTime UTCtime = TimeHelper.GetUTCTime(LocalTime, Convert.ToDouble(LocalTimeZome));
            DateTime Relativetime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(UTCtime, local.Id);
            result = TimeZoneInfo.Local.IsDaylightSavingTime(Relativetime);
            return result;
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
        private double MathWorkTime(double _v)
        {
            double value = (_v >= 0) ? _v : 0;
            return value;
        }
    }
}
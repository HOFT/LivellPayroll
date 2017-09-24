using LivellPayRoll.App_Helpers;
using LivellPayRoll.Enum;
using LivellPayRoll.Infrastructure;
using LivellPayRoll.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LivellPayRoll.Controllers
{
    [Authorize]
    [CustomAuthorize]
    public class SystemController : Controller
    {
        // GET: System
        [Authorize(Roles = "Governor")]
        public ActionResult UserControl()
        {
            AppUser User = LoginUser;
            Dictionary<string, string> StatusDic = EnumHelper.GetEnumItemValueDesc(typeof(Status));
            var companyList = db.Company.Where(t => true).OrderByDescending(t => t.PayRollRegTime).ToList();
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Id");
            dataTable.Columns.Add("CompanyName");
            dataTable.Columns.Add("Contact");
            dataTable.Columns.Add("Phone");
            dataTable.Columns.Add("EmployeeNum");
            dataTable.Columns.Add("LastLogin");
            dataTable.Columns.Add("RegTime");
            dataTable.Columns.Add("Status");
            foreach (var t in companyList)
            {
                if (t.Email == "Governor@livellpayroll.com") {
                    continue;
                };
                DataRow dr = dataTable.NewRow();
                dr["Id"] = t.CompanyId;
                dr["CompanyName"] = t.CompanyName;
                dr["Contact"] = t.ContactName;
                dr["Phone"] = t.Telphone;

                var EmployeeList = db.Employee.Where(x => x.CompanyId == t.CompanyId).ToList();
                dr["EmployeeNum"] = EmployeeList.Count;

                var LastLogin = db.Users.Where(u => u.CompanyId == t.CompanyId).Max(u => u.LastLoginDate);
                dr["LastLogin"] = TimeHelper.GetLocalTime(LastLogin, double.Parse(User.TimeZone)).ToString();
                dr["RegTime"] = TimeHelper.GetLocalTime(t.PayRollRegTime,double.Parse(User.TimeZone)).ToString("yyyy-MM-dd");
                string Status = "";
                if (t.Status == "0")
                {
                    Status = "<span class='center-block padding-5 label label-success'>" + StatusDic[t.Status] + "</span>";
                }
                else if (t.Status == "2")
                {
                    Status = "<span class='center-block padding-5 label label-warning'>" + StatusDic[t.Status] + "</span>";
                }
                else if (t.Status == "3")
                {
                    Status = "<span class='center-block padding-5 label label-default'>" + StatusDic[t.Status] + "</span>";
                }
                else {
                    Status = "<span class='center-block padding-5 label label-info'>" + StatusDic[t.Status] + "</span>";
                }
                dr["Status"] = Status;
                dataTable.Rows.Add(dr);
            }
            dataTable.DefaultView.Sort = "RegTime asc";
            return View(dataTable);
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
    }
}
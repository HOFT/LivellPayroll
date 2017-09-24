using LivellPayRoll.Infrastructure;
using LivellPayRoll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LivellPayRoll.App_Helpers
{
    public class SystemVariates
    {
        //AppIdentityDbContext db = DbContextFactory.DbCon();
        //string UserName = System.Web.HttpContext.Current.User.Identity.Name;
        public static string LoginUserName
        {
            get
            {
                //var a = System.Web.HttpContext.Current.User.Identity.Name;
                return HttpContext.Current.Request.Cookies.Get("LoginInfo").Values["UserName"];
            }
        }
        public static double TimeZone
        {
            get
            {
                return int.Parse(HttpContext.Current.Request.Cookies.Get("LoginInfo").Values["TimeZone"]);
            }
        }
        public static string LoginRoleId
        {
            get
            {
                //var a = System.Web.HttpContext.Current.User.Identity.Name;
                if (HttpContext.Current.Request.Cookies.Get("LoginInfo") != null)
                    return HttpContext.Current.Request.Cookies.Get("LoginInfo").Values["RoleId"];
                else
                    return "";

            }
        }
        public static string LoginRoleName
        {
            get
            {
                //var a = System.Web.HttpContext.Current.User.Identity.Name;
                return HttpContext.Current.Request.Cookies.Get("LoginInfo").Values["RoleName"];
            }
        }

    }
}
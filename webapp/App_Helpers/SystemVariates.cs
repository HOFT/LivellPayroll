using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LivellPayRoll.App_Helpers
{
    public class SystemVariates
    {
        public static string LoginUserName
        {
            get
            {
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
    }
}
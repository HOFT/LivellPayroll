using LivellPayRoll.Infrastructure;
using LivellPayRoll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LivellPayRoll.App_Helpers
{
    public class TimeZones
    {
        public static Dictionary<string, object>  DicTimeZones()
        {
            Dictionary<string, object> dicTZ = new Dictionary<string, object>();
            using (AppIdentityDbContext db=new AppIdentityDbContext())
            {
                var timezones = db.DM_TimeZone.Where<DM_TimeZone>(t => true).OrderBy<DM_TimeZone, int>(u => u.id).ToList();
                foreach (var tz in timezones)
                {
                    dicTZ.Add(tz.TimeZone, float.Parse(tz.Code));
                }
            }
            return dicTZ;
        }
    }
}
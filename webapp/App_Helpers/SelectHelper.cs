using LivellPayRoll.Infrastructure;
using LivellPayRoll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LivellPayRoll.App_Helpers
{
    public class SelectHelper
    {
        public static SelectList TimeZoneToSelect(AppIdentityDbContext db)
        {
            var Tlist = db.DM_TimeZone.Where<DM_TimeZone>(t => true).OrderBy(u => u.id).ToList();

            List<SelectListItem> TZ = new List<SelectListItem>();
            foreach (var t in Tlist)
            {
                TZ.Add(new SelectListItem() { Text = t.TimeZone, Value = t.Code });
            }
            return new SelectList(TZ, "Value", "Text");
        }
        public static SelectList JobToSelect(AppIdentityDbContext db, List<Job> Tlist)
        {
            List<SelectListItem> List = new List<SelectListItem>();
            foreach (var t in Tlist)
            {
                List.Add(new SelectListItem() { Text = t.JobName, Value = t.JobId.ToString() });
            }
            return new SelectList(List, "Value", "Text");
        }
    }
}
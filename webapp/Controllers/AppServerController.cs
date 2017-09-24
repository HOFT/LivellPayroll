using LivellPayRoll.Infrastructure;
using LivellPayRoll.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LivellPayRoll.Controllers
{
    public class AppServerController : Controller
    {
        // GET: AppServer
        public JsonResult GetTimeList()
        {
            var dataList = db.TimeSheet.Where(t => 1 == 1).Select(t => new { Note = t.Note, TotalWorkTime = t.TotalWorkTime }).ToList();
            //ajax请求必须加
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            HttpContext.Response.AppendHeader("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
            return Json(new { dataList, result = true }, JsonRequestBehavior.AllowGet);
        }
        
        private AppIdentityDbContext db
        {
            get
            {
                return HttpContext.GetOwinContext().Get<AppIdentityDbContext>();
            }
        }
    }

}
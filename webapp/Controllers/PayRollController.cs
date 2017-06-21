using LivellPayRoll.App_Helpers;
using LivellPayRoll.Enum;
using LivellPayRoll.Infrastructure;
using LivellPayRoll.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LivellPayRoll.Controllers
{
    [Authorize]
    public class PayRollController : Controller
    {

        public ActionResult PayRollList()
        {
            var t105=db.T105.Where(t => t.CompanyId == LoginUser.Company.CompanyId).ToList();
            return View(t105);
        }
        public ActionResult PayRollDelete(System.Guid Id) {
            T105 t = db.T105.Find(Id);
            db.Entry<T105>(t).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return Json(new { code = "1", message = "success!" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PayRollInput()
        {
            Company c = db.Company.Find(LoginUser.Company.CompanyId);
            ViewBag.Period = ((Period)(int.Parse(c.PayFreq))).ToString();

            var List102 = db.T102.Where<T102>(t => t.CompanyId == LoginUser.Company.CompanyId).OrderBy(t => t.ItemId).ToList();
            ViewBag.List102 = List102;
            var List201 = db.T201.Where<T201>(t => t.CompanyId == LoginUser.Company.CompanyId && t.Type == 1).OrderBy(t => t.Ord).ToList();
            ViewBag.List201 = List201;
            return View();
        }
        public JsonResult GetEmployee()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            var Employees = db.Employee.Where<Employee>(e => e.CompanyId == LoginUser.Company.CompanyId).ToList();
            foreach (var e in Employees)
            {
                items.Add(new SelectListItem() { Text = e.LName + " " + e.FName + " [" + e.Email + "]", Value = e.EmployeeId.ToString() });
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PayRollShow() {
            Dictionary<string, bool> dic = new Dictionary<string, bool>();
            var List102 = db.T102.Where<T102>(t => t.CompanyId == LoginUser.Company.CompanyId).OrderBy(t => t.ItemId).ToList();
            var List201 = db.T201.Where<T201>(t => t.CompanyId == LoginUser.Company.CompanyId && t.Type == 1).OrderBy(t => t.Ord).ToList();
            foreach (var r in List102) {
                dic.Add(r.CodeMap, r.Enabled);
            }
            foreach (var r in List201)
            {
                dic.Add(r.CodeMap, r.Enabled);
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
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
using LivellPayRoll.App_Helpers;
using LivellPayRoll.Enum;
using LivellPayRoll.Infrastructure;
using LivellPayRoll.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LivellPayRoll.Controllers
{
    [Authorize]
    [CustomAuthorize]
    public class PayRollSetupController : Controller
    {
        // GET: PayRollSetup
        public ActionResult PayRollSet()
        {
            Company c = db.Company.Find(LoginUser.CompanyId);
            Dictionary<string, string> StaList = EnumHelper.GetEnumItemDesc(typeof(States));
            ViewBag.StatesList = new SelectList(StaList, "key", "value");
            Dictionary<string, string> Period = EnumHelper.GetEnumItemDesc(typeof(Period));
            ViewBag.Period = new SelectList(Period, "key", "value");
            Dictionary<string, object> RoundTo = EnumHelper.EnumListDic<RoundTo>("", "");
            ViewBag.RoundTo = new SelectList(RoundTo, "value", "key");
            ViewBag.TimeZone = SelectHelper.TimeZoneToSelect(db);
            var em = db.Employee.Where(t => t.UserRole != "Employee").ToList();
            List<SelectListItem> Contact = new List<SelectListItem>();
            foreach (var e in em) {
                Contact.Add(new SelectListItem() { Text = e.AppUser.PayRollUser + " [ " + e.AppUser.Email + " ]", Value = e.AppUser.Id });
            }
            ViewBag.Contact = new SelectList(Contact, "Value", "Text");
            return View(c);
        }
        [HttpPost]
        public ActionResult PayRoll(Company c)
        {
            Company company = db.Company.Find(LoginUser.CompanyId);
            
            if (company.Email != c.Email) {
                
                if (company.ContactName != c.ContactName) {
                    AppUser OldAdminUser = UserManager.FindById(company.ContactName);
                    UserManager.RemoveFromRole(OldAdminUser.Id,"Admin");
                    UserManager.AddToRole(OldAdminUser.Id, "Manager");
                    Employee e1 = OldAdminUser.Employee.FirstOrDefault();
                    if (e1 != null) {
                        e1.UserRole = "Manager";
                    }
                    db.Entry<Employee>(e1).State = System.Data.Entity.EntityState.Modified;

                    AppUser NewAdmin = UserManager.FindById(c.ContactName);
                    UserManager.RemoveFromRole(OldAdminUser.Id, "Manager");
                    UserManager.AddToRole(OldAdminUser.Id, "Admin");
                    Employee e2 = OldAdminUser.Employee.FirstOrDefault();
                    if (e2 != null)
                    {
                        e2.UserRole = "Admin";
                    }
                    db.Entry<Employee>(e2).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                //检查用户Email是否被占用
                AppUser NewUser = UserManager.FindByEmail(c.Email);
                if (NewUser != null) {
                    return Json(new { code="0", message = "The Email Account Has Been Used, Please Input Again!" }, JsonRequestBehavior.AllowGet);
                }
                AppUser user = UserManager.FindByEmail(company.Email);
                if (user == null)
                {
                    user = new AppUser { Email = c.Email, UserName = c.Email, CompanyId = LoginUser.CompanyId, TimeZone = c.TimeZone, PayRollUser = c.ContactName };
                    UserManager.Create(user, "123456");
                }else {
                    user.Email = c.Email;
                    user.UserName = c.Email;
                    user.TimeZone = c.TimeZone;
                    user.PayRollUser = c.ContactName;
                    var result = UserManager.UpdateAsync(user);
                }
                if (!UserManager.IsInRole(user.Id, "Admin"))
                {
                    UserManager.AddToRole(user.Id, "Admin");
                }
            }
            company.CompanyName = c.CompanyName;
            company.TradeName = c.TradeName;
            company.FedTaxId = c.FedTaxId;
            company.Address1 = c.Address1;
            company.Address2 = c.Address2;
            company.City = c.City;
            company.State = c.State;
            company.Zip = c.Zip;
            company.WWW = c.WWW;
            company.ContactName = c.ContactName;
            company.Telphone = c.Telphone;
            company.Fax = c.Fax;
            company.Email = c.Email;
            company.TimeZone = c.TimeZone;
            company.RoundTo = c.RoundTo;
            company.PayFreq = c.PayFreq;
            company.PayReportByEndingDate = c.PayReportByEndingDate;
            company.DaylightSavingTime = c.DaylightSavingTime;
            db.Entry(company).State = System.Data.Entity.EntityState.Modified;
            foreach (var user in company.AppUser) {
                if (UserManager.IsInRole(user.Id, "Admin")|| UserManager.IsInRole(user.Id, "Manager"))
                {
                    user.TimeZone = c.TimeZone;
                }
            }
            db.SaveChanges();
            return Json(new { code = "1", message = "Success!" }, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("PayRoll", new { code = 1, message = "Customer been successfully added!" });
        }
        public ActionResult Tax()
        {
            Company c = db.Company.Find(LoginUser.CompanyId);
            return View(c);
        }
        [HttpPost]
        public ActionResult Tax(Company c)
        {
            Company company = db.Company.Find(LoginUser.CompanyId);
            company.FedTaxId = c.FedTaxId;
            company.ControlNo = c.ControlNo;
            company.Establish = c.Establish;
            company.FUTA = c.FUTA;
            company.StateID = c.StateID;
            company.StateUnemWage = c.StateUnemWage;
            company.SUTA = c.SUTA;
            db.Entry(company).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Tax");
        }
        public ActionResult Deduction()
        {
            var t102 = db.T102.Where<T102>(t => t.CompanyId == LoginUser.CompanyId).OrderBy<T102, int>(t => t.ItemId).ToList();
            return View(t102);
        }
        [HttpPost]
        public ActionResult Deduction(bool[] valuesData)
        {
            var t102 = db.T102.Where<T102>(t => t.CompanyId == LoginUser.CompanyId).OrderBy<T102, int>(t => t.ItemId).ToList();
            foreach (var t in t102) {
                int index = t102.IndexOf(t);
                t.Enabled = valuesData[index];
            }
            db.SaveChanges();
            return Json(new { code = "1", message = "Success!" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeductionUpdate(T102 t) {
            bool Taxable = (Request["Taxable"] == "on") ? true : false;
            bool FICATaxable = (Request["FICATaxable"] == "on") ? true : false;
            bool PctofIncome = (Request["PctofIncome"] == "on") ? true : false;
            bool W2Box10 = (Request["W2Box10"] == "on") ? true : false;
            bool W2Box12 = (Request["W2Box12"] == "on") ? true : false;
            T102 tItem = db.T102.Find(t.Id);
            tItem.Description = t.Description;
            tItem.AnnualLimit = t.AnnualLimit;
            tItem.Taxable = Taxable;
            tItem.FICATaxable = FICATaxable;
            tItem.PctofIncome = PctofIncome;
            tItem.W2Box10 = W2Box10;
            tItem.W2Box12 = W2Box12;
            tItem.W2Code = t.W2Code;
            db.Entry(tItem).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Deduction");
        }
        [HttpPost]
        public JsonResult DeductionQuery(Guid Id) {
            var list = db.T102.Where(t => t.Id == Id).Select(t => new { Id = t.Id, Description = t.Description, AnnualLimit=t.AnnualLimit, PctofIncome=t.PctofIncome, Taxable=t.Taxable, FICATaxable=t.FICATaxable, W2Box10=t.W2Box10, W2Box12=t.W2Box12, W2Code=t.W2Code }).ToList();
            return Json(list);
        }
        public ActionResult Wage() {
            var t201 = db.T201.Where<T201>(t => t.CompanyId == LoginUser.CompanyId).OrderBy<T201, int>(t => t.Ord).ToList();
            return View(t201);
        }
        [HttpPost]
        public ActionResult Wage(bool[] valuesData)
        {
            var t201 = db.T201.Where<T201>(t => t.CompanyId == LoginUser.CompanyId).OrderBy<T201, int>(t => t.Ord).ToList();
            foreach (var t in t201)
            {
                int index = t201.IndexOf(t);
                t.Enabled = valuesData[index];
            }
            db.SaveChanges();
            return Json(new { code = "1", message = "Success!" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult WageQuery(Guid Id)
        {
            var list = db.T201.Where(t => t.Id == Id).Select(t => new { Id = t.Id, Description = t.Description }).ToList();
            return Json(list);
        }
        [HttpPost]
        public ActionResult WageUpdate(T201 t)
        {
            T201 tItem = db.T201.Find(t.Id);
            tItem.Description = t.Description;
            db.Entry(tItem).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Wage");
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
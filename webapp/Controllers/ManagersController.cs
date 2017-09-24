using LivellPayRoll.Infrastructure;
using LivellPayRoll.Models;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using System;
using LivellPayRoll.App_Helpers;

namespace LivellPayRoll.Controllers
{
    [Authorize]
    //[Authorize(Roles = "Manager")]
    [CustomAuthorize]
    public class ManagersController : Controller
    {
        // GET: Managers
        public ActionResult Mlist()
        {
            var users = db.Users.Where(a => a.CompanyId == LoginUser.CompanyId && a.Roles.All(r => r.RoleId == "R02")).ToList();

            return View(users);
        }
        public ActionResult AddManager() {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Add(ManagerUserMode viewModel) {
            if (!ModelState.IsValid)
                return View(viewModel);
            AppUser Admin = LoginUser;
            AppUser user = UserManager.FindByEmail(viewModel.Email);
            if (user != null) {
                ModelState.AddModelError("", "The email has been registered!");
                return View(viewModel);
            }
            user = new AppUser
            {
                UserName = viewModel.Email,
                Email = viewModel.Email,
                PayRollUser = viewModel.UserName,
                PhoneNumber = viewModel.Phone,
                TimeZone = Admin.TimeZone,
                CompanyId = Admin.CompanyId
            };
            var result = await UserManager.CreateAsync(user, "Pay123456");
            if (result.Succeeded)
            {
                if (!UserManager.IsInRole(user.Id, "Manager"))
                {
                    UserManager.AddToRole(user.Id, "Manager");
                }

                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = Url.Action("UserConfirm", "Account", new { userId = user.Id, code = code, Nt = TimeHelper.ConvertDateTimeInt(DateTime.UtcNow) }, protocol: Request.Url.Scheme);
                string EmailBody = "Thank you for creating an PayRoll account. You need to confirm your account and setting your password, you'll have access to PayRoll system.";
                string EmailLink = "Please confirm your account complete regist by clicking.<a href=\"" + callbackUrl + "\">Click it! &raquo;</a>";
                string strbody = ReplaceText(EmailBody, EmailLink);
                await UserManager.SendEmailAsync(user.Id, "Confirm Your Account  (" + user.PayRollUser + ")", strbody);

            }
            return RedirectToAction("Mlist");
        }
        [HttpPost]
        public ActionResult Delete(string Id)
        {
            AppUser user = UserManager.FindById(Id);
            if (user != null) {
                UserManager.Delete(user);
            }
            return Json(new {code=1, message= "Manager been successfully Deleted!" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditManager(string Id)
        {
            AppUser user = UserManager.FindById(Id);
            ManagerUserMode mode = new ManagerUserMode
            {
                UserId = user.Id,
                Email = user.Email,
                Phone = user.PhoneNumber,
                UserName = user.PayRollUser
            };
            return View(mode);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(ManagerUserMode viewModel)
        {
            AppUser user = UserManager.FindById(viewModel.UserId);
            user.PayRollUser = viewModel.UserName;
            user.PhoneNumber = viewModel.Phone;
            var result = await UserManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                // Add all errors to the page so they can be used to display what went wrong
                AddErrors(result);
                return View(viewModel);
            }
            return RedirectToAction("Mlist");
        }
        private void AddErrors(IdentityResult result)
        {
            // Add all errors that were returned to the page error collection
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
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
        private AppRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppRoleManager>();
            }
        }
        private string ReplaceText(string Email_Body, string Email_Link)
        {

            string path = string.Empty;

            path = HttpContext.Server.MapPath("~/App_Helpers/EmailTemplate.html");

            if (path == string.Empty)
            {
                return string.Empty;
            }
            System.IO.StreamReader sr = new System.IO.StreamReader(path);
            string str = string.Empty;
            str = sr.ReadToEnd();
            str = str.Replace("$Email_Body$", Email_Body);
            str = str.Replace("$Email_Link$", Email_Link);

            return str;
        }
    }
}
#region Using

using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using LivellPayRoll.Models;
using LivellPayRoll.Infrastructure;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System;
using System.Collections.Generic;
using LivellPayRoll.App_Helpers;
using LivellPayRoll.Enum;
using System.Net;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security;
#endregion

namespace LivellPayRoll.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // TODO: This should be moved to the constructor of the controller in combination with a DependencyResolver setup
        // NOTE: You can use NuGet to find a strategy for the various IoC packages out there (i.e. StructureMap.MVC5)
        //private readonly UserManager _manager = UserManager.Create();
        

        // GET: /account/forgotpassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            // We do not want to use any existing identity information
            EnsureLoggedOut();

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(AccountForgotPasswordModel viewModel)
        {
            AppUser user = UserManager.FindByEmail(viewModel.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "The user is not found.");
                return View(viewModel);
            };
            if (!await UserManager.IsEmailConfirmedAsync(user.Id))
            {
                return RedirectToAction("ConfirmedEmail", "Account", new { Email = user.Email });
            }
            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            //code = HttpUtility.UrlEncode(code);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code, Nt= TimeHelper.ConvertDateTimeInt(DateTime.UtcNow) }, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(
                user.Id,
               "Password Reset – Livell PayRoll (" + user.Email + ")",
               "Dear LivellPayRoll Customer:<br><br> Please resetting your password by clicking <a href=\"" + callbackUrl + "\">link</a><br><br> —The LivellPayRoll Team");

            return View(viewModel);
        }
        [AllowAnonymous]
        public ActionResult ResetPassword(string userId, string code, string Nt) {
            //链接过期
            double timeInt = Convert.ToDouble(Nt);
            if (timeInt + 3 * 3600 <= TimeHelper.ConvertDateTimeInt(DateTime.UtcNow)|| timeInt > TimeHelper.ConvertDateTimeInt(DateTime.UtcNow) || timeInt == 0) {
                return RedirectToLocal();
            }
            EnsureLoggedOut();
            //HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
            if (userId == null || code == null)
            {
                return RedirectToLocal();
            }
            AccountResetPasswordModel mode = new AccountResetPasswordModel() { Code = code, UserId = userId };
            AppUser user = UserManager.FindById(userId);
            ViewBag.UserInfo = user.Email + "  [ " + user.PayRollUser + " ]";
            return View(mode);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(AccountResetPasswordModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            AppUser user = UserManager.FindById(viewModel.UserId);
            if (user == null) {
                return RedirectToAction("Register", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(viewModel.UserId, viewModel.Code, viewModel.Password);
            if (result.Succeeded)
            {
                return RedirectToLocal();
            }
            AddErrors(result);
            return View(viewModel);
        }

        // GET: /account/login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            // We do not want to use any existing identity information
            EnsureLoggedOut();
            // Store the originating URL so we can attach it to a form field
            var viewModel = new AccountLoginModel { ReturnUrl = returnUrl };
            return View(viewModel);
        }

        // POST: /account/login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(AccountLoginModel viewModel, string ReturnUrl)
        {
            // Ensure we have a valid viewModel to work with
            if (!ModelState.IsValid)
                return View(viewModel);

            var user = await UserManager.FindByEmailAsync(viewModel.Email);
            if (user != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    return RedirectToAction("ConfirmedEmail","Account",  new { Email= user.Email });
                }
                var result = await ApplicationSignInManager.PasswordSignInAsync(user.UserName, viewModel.Password, viewModel.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        SignCookieAsync(user);
                        return RedirectToLocal(ReturnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = ReturnUrl, RememberMe = viewModel.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid username or password.");
                        return View(viewModel);
                }
            }
            else {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(viewModel);
            }
        }

        // GET: /account/error
        [AllowAnonymous]
        public ActionResult Error()
        {
            // We do not want to use any existing identity information
            EnsureLoggedOut();

            return View();
        }

        // GET: /account/register
        [AllowAnonymous]
        public ActionResult Register()
        {
            // We do not want to use any existing identity information
            Dictionary<string, object> StaList = EnumHelper.EnumListDic<States>("", "");
            ViewBag.StatesList = new SelectList(StaList, "value", "key");
            Dictionary<string, object> TZList = TimeZones.DicTimeZones();
            ViewBag.TimeZone = new SelectList(TZList, "value", "key");

            EnsureLoggedOut();
            return View(new AccountRegistrationModel());
            //return View();
        }

        // POST: /account/register
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(AccountRegistrationModel viewModel)
        {
            Dictionary<string, object> StaList = EnumHelper.EnumListDic<States>("", "");
            ViewBag.StatesList = new SelectList(StaList, "value", "key");
            Dictionary<string, object> TZList = TimeZones.DicTimeZones();
            ViewBag.TimeZone = new SelectList(TZList, "value", "key");
            // Ensure we have a valid viewModel to work with
            if (!ModelState.IsValid)
                return View(viewModel);

            // Prepare the identity with the provided information
            DateTime dt = DateTime.Now;
            Company company = new Company
            {
                CompanyName = viewModel.CompanyName,
                Address1 = viewModel.Address,
                City = viewModel.City,
                State = viewModel.State,
                Telphone = viewModel.Telphone,
                TimeZone = viewModel.TimeZone,
                Email = viewModel.Email,
                PayFreq = "1",
                Country = "United States",
                RoundTo = "15",
                PayRollRegTime = dt
            };
            var user = new AppUser
            {
                UserName = dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString() + dt.Hour.ToString() + dt.Millisecond.ToString(),
                Email = viewModel.Email,
                TimeZone= viewModel.TimeZone,
                Company= company
            };
            

            // Try to create a user with the given identity
            try
            {
                var result = await UserManager.CreateAsync(user, "Pay123456");
                // If the user could not be created
                if (!result.Succeeded) {
                    // Add all errors to the page so they can be used to display what went wrong
                    AddErrors(result);
                    return View(viewModel);
                }

                if (!UserManager.IsInRole(user.Id, "Admin"))
                {
                    UserManager.AddToRole(user.Id, "Admin");
                }
                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = Url.Action("CompleteRegist", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(
                    user.Id,
                   "Confirm Your Account – Complete Registration Information (" + user.Company.CompanyName + ")",
                   "Dear LivellPayRoll Customer:<br><br>Thank you for creating an PayRoll account. You need to complete the registration information and confirm your account, you'll have access to PayRoll system. Please confirm your account complete regist by clicking this link: <a href=\"" + callbackUrl + "\">link</a><br><br>Welcome to the LivellPayRoll community!<br><br>—The LivellPayRoll Team");
                
                return RedirectToLocal();
            }
            catch (DbEntityValidationException ex)
            {
                // Add all errors to the page so they can be used to display what went wrong
                AddErrors(ex);

                return View(viewModel);
            }
        }

        // POST: /account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {

            // First we clean the authentication ticket like always
            AuthenticationManager.SignOut();
            FormsAuthentication.SignOut();
            // Second we clear the principal to ensure the user does not retain any authentication
            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

            // Last we redirect to a controller/action that requires authentication to ensure a redirect takes place
            // this clears the Request.IsAuthenticated flag since this triggers a new request
            return RedirectToLocal();
        }

        private ActionResult RedirectToLocal(string returnUrl = "")
        {
            // If the return url starts with a slash "/" we assume it belongs to our site
            // so we will redirect to this "action"
            if (!returnUrl.IsNullOrWhiteSpace() && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            // If we cannot verify if the url is local to our host we redirect to a default location
            return RedirectToAction("index", "home");
        }

        private void AddErrors(DbEntityValidationException exc)
        {
            foreach (var error in exc.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors.Select(validationError => validationError.ErrorMessage)))
            {
                ModelState.AddModelError("", error);
            }
        }

        private void AddErrors(IdentityResult result)
        {
            // Add all errors that were returned to the page error collection
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private void EnsureLoggedOut()
        {
            // If the request is (still) marked as authenticated we send the user to the logout action
            if (Request.IsAuthenticated)
                Logout();
        }
        private void SignCookieAsync(AppUser user) {
            HttpCookie cookie = new HttpCookie("LoginInfo");
            cookie.Expires = DateTime.Now.AddDays(1);
            cookie["UserName"] = user.PayRollUser;
            cookie["TimeZone"] = user.TimeZone.ToString();
            Response.Cookies.Add(cookie);
        }
        private async Task SignInAsync(AppUser user, bool isPersistent)
        {
            // Clear any lingering authencation data
            FormsAuthentication.SignOut();

            // Create a claims based identity for the current user
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            // Write the authentication cookie
            FormsAuthentication.SetAuthCookie(identity.Name, isPersistent);

            //var userrole = user.Roles.FirstOrDefault();
            //HttpCookie cookie = new HttpCookie("LoginInfo");
            //cookie.Expires = DateTime.Now.AddDays(1);
            //cookie["UserId"] = user.Id;
            //cookie["UserName"] = user.UserName;
            //cookie["Email"] = user.Email;
            //cookie["RoleId"] = userrole.RoleId;
            //cookie["CompanyId"] = user.Company.CompanyId.ToString();
            //cookie["TimeZone"] = user.TimeZone.ToString();
            //cookie["RoundTo"] = user.Company.RoundTo.ToString();
            //Response.Cookies.Add(cookie);
        }
        [AllowAnonymous]
        public ActionResult ConfirmedEmail(string Email)
        {
            EnsureLoggedOut();
            ViewBag.ConfirmedEmail = Email;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResendEmail(string Email)
        {
                var user = await UserManager.FindByEmailAsync(Email);
                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                //code = HttpUtility.UrlEncode(code);
                var callbackUrl = Url.Action("CompleteRegist", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(
                    user.Id,
                   "Confirm Your Account – Complete Registration Information (" + user.Company.CompanyName + ")",
                   "Dear LivellPayRoll Customer:<br><br>Thank you for creating an PayRoll account. You need to complete the registration information and confirm your account, you'll have access to PayRoll system. Please confirm your account complete regist by clicking this link: <a href=\"" + callbackUrl + "\">link</a><br><br>Welcome to the LivellPayRoll community!<br><br>—The LivellPayRoll Team");
            return Json(new { code= "code", message= "success" }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> CompleteRegist(string userId, string code)
        {

            EnsureLoggedOut();
            //EnsureLoggedOut();
            Dictionary<string, object> StaList = EnumHelper.EnumListDic<States>("", "");
            ViewBag.StatesList = new SelectList(StaList, "value", "key");
            Dictionary<string, object> RoundTo = EnumHelper.EnumListDic<RoundTo>("", "");
            ViewBag.RoundTo = new SelectList(RoundTo, "value", "key");
            Dictionary<string, object> TZList = TimeZones.DicTimeZones();
            ViewBag.TimeZone = new SelectList(TZList, "value", "key");

            if (userId == null || code == null) {
                return RedirectToLocal();
            }
            if (await UserManager.IsEmailConfirmedAsync(userId)) {
                return RedirectToLocal();
            }
            AppUser user = await UserManager.FindByIdAsync(userId);
            CompleteRegistModel mode = new CompleteRegistModel()
            {
                code = code,
                Email =user.Company.Email,
                CompanyName =user.Company.CompanyName
            };
            
            return View(mode);
        }
        [HttpPost]
        [AllowAnonymous]

        public async Task<ActionResult> CompleteRegist(CompleteRegistModel viewModel) {
            // Ensure we have a valid viewModel to work with
            if (!ModelState.IsValid)
                return View(viewModel);

            var user = await UserManager.FindByEmailAsync(viewModel.Email);
            if (user != null) {
                //user.EmailConfirmed = true;
                user.PayRollUser = viewModel.ContactName;
                user.UserName = viewModel.Email;
                user.Company.ContactName = viewModel.ContactName;
                //user.PasswordHash= UserManager.PasswordHasher.HashPassword(viewModel.Password);
                //var result2 = await UserManager.ConfirmEmailAsync(user.Id, viewModel.code);
                IdentityResult result = await UserManager.ConfirmEmailAsync(user.Id, viewModel.code);
                if (result.Succeeded)
                {
                    //await UserManager.ResetPasswordAsync(user.Id, viewModel.code, viewModel.Password);
                    var resultPW = await UserManager.ChangePasswordAsync(user.Id, "Pay123456", viewModel.Password);
                    if (!resultPW.Succeeded) {
                        AddErrors(resultPW);
                        return View(viewModel);
                    }
                    var resultUp = await UserManager.UpdateAsync(user);
                    if (resultUp.Succeeded)
                    {
                        //await SignInAsync(user, true);
                        await ApplicationSignInManager.PasswordSignInAsync(user.UserName, viewModel.Password, true, shouldLockout: false);
                        SignCookieAsync(user);
                        return RedirectToLocal();
                    }
                    else
                    {
                        AddErrors(resultUp);
                        return View(viewModel);
                    }
                }
                else
                {
                    AddErrors(result);
                    return View(viewModel);
                }
                
            }
                return View(viewModel);
        }
        // GET: /account/lock
        [AllowAnonymous]
        public ActionResult Lock()
        {
            return View();
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
        private ApplicationSignInManager ApplicationSignInManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
        }
        private AppUser LoginUser {
            get
            {
                //AppUser user = HttpContext.User;
                AppUser user = UserManager.FindByName(HttpContext.User.Identity.Name);
                return user;
            }
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}
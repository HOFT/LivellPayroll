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
using System.Web.Script.Serialization;
#endregion

namespace LivellPayRoll.Controllers
{
    [Authorize]
    [CustomAuthorize]
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
            
            string EmailBody = "Please resetting your password by clicking.";
            string EmailLink = "Please resetting your password by clicking.<a href=\""+ callbackUrl + "\">Click it! &raquo;</a>";
            string strbody = ReplaceText( EmailBody, EmailLink);
            await UserManager.SendEmailAsync(user.Id, "Password Reset – Livell PayRoll (" + user.Email + ")", strbody);

            //await UserManager.SendEmailAsync(
            //    user.Id,
            //   "Password Reset – Livell PayRoll (" + user.Email + ")",
            //   "Dear LivellPayRoll Customer:<br><br> Please resetting your password by clicking <a href=\"" + callbackUrl + "\">link</a><br><br> —The LivellPayRoll Team");
            ViewBag.ReceiveEmail = user.Email;
            return View("WaitEmail");
        }
        [AllowAnonymous]
        public ActionResult ResetPassword(string userId, string code, string Nt) {
            //链接过期
            double timeInt = Convert.ToDouble(Nt);
            if (timeInt + 3 * 3600 <= TimeHelper.ConvertDateTimeInt(DateTime.UtcNow)|| timeInt > TimeHelper.ConvertDateTimeInt(DateTime.UtcNow) || timeInt == 0) {
                return RedirectToAction("Error404","Account",new { ErrorMessage = "Link Has Expired" } );
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
                Logout();
                var result = await ApplicationSignInManager.PasswordSignInAsync(user.UserName, viewModel.Password, viewModel.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        {
                            if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                            {
                                return RedirectToAction("ConfirmedEmail", "Account", new { Email = user.Email });
                            }
                            user.LastLoginDate = DateTime.UtcNow;
                            await UserManager.UpdateAsync(user);
                            SignCookieAsync(user);
                            return RedirectToLocal(ReturnUrl);

                        }
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
            Dictionary<string, string> StaList = EnumHelper.GetEnumItemDesc(typeof(States));
            ViewBag.StatesList = new SelectList(StaList, "key", "value");
            //Dictionary<string, object> StaList = EnumHelper.EnumListDic<States>("", "");
            //ViewBag.StatesList = new SelectList(StaList, "value", "key");
            ViewBag.TimeZone = SelectHelper.TimeZoneToSelect(db);

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
            Dictionary<string, string> StaList = EnumHelper.GetEnumItemDesc(typeof(States));
            ViewBag.StatesList = new SelectList(StaList, "key", "value");
            ViewBag.TimeZone = SelectHelper.TimeZoneToSelect(db); ;
            // Ensure we have a valid viewModel to work with
            if (!ModelState.IsValid)
                return View(viewModel);

            // Prepare the identity with the provided information
            DateTime dt = DateTime.UtcNow;
            Company company = new Company
            {
                CompanyName = viewModel.CompanyName,
                Address1 = viewModel.Address,
                City = viewModel.City,
                State = viewModel.State,
                Telphone = viewModel.Telphone,
                TimeZone = viewModel.TimeZone,
                Email = viewModel.Email,
                Zip = viewModel.Zip,
                TradeName = viewModel.TradeName,
                PayFreq = "Weekly",
                Country = "United States",
                RoundTo = "15",
                PayRollRegTime = dt,
                WeekRule = true,
                WeekRuleValue = 40,
                DayRule = true,
                DayRuleValue = 8,
                DoubeRule = true,
                DoubeRuleValue = 12,
                DaylightSavingTime = true,
                Status = "3"
            };
            var user = new AppUser
            {
                UserName = dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString() + dt.Hour.ToString() + dt.Millisecond.ToString(),
                Email = viewModel.Email,
                TimeZone= viewModel.TimeZone,
                LastLoginDate = DateTime.UtcNow,
                Company = company
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
                var callbackUrl = Url.Action("CompleteRegist", "Account", new { userId = user.Id, code = code, Nt = TimeHelper.ConvertDateTimeInt(DateTime.UtcNow) }, protocol: Request.Url.Scheme);

                string EmailBody = "Thank you for creating an PayRoll account. You need to complete the registration information and confirm your account, you'll have access to PayRoll system.";
                string EmailLink = "Please confirm your account complete regist by clicking.<a href=\"" + callbackUrl + "\">Click it! &raquo;</a>";
                string strbody = ReplaceText(EmailBody, EmailLink);
                await UserManager.SendEmailAsync(user.Id, "Confirm Your Account – Complete Registration Information (" + user.Company.CompanyName + ")", strbody);

                ViewBag.ReceiveEmail = user.Email;
                return View("WaitEmail");
                //return RedirectToLocal();
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
        [AllowAnonymous]
        public ActionResult Error404(string ErrorMessage)
        {
            // We do not want to use any existing identity information
            
            EnsureLoggedOut();
            ViewBag.ErrorMessage = ErrorMessage;
            return View();
        }
        [Authorize]
        [CustomAuthorize]
        public ActionResult GeneralSet()
        {
            Dictionary<string, string> StaList = EnumHelper.GetEnumItemDesc(typeof(States));
            ViewBag.StatesList = new SelectList(StaList, "key", "value");
            ViewBag.TimeZoneList = SelectHelper.TimeZoneToSelect(db);
            ViewBag.DefaultJobList = new SelectList("", "Value", "Text");
            AppUser user = LoginUser;
            GeneralSetModel mode = new GeneralSetModel
            {
                RoleId = user.Roles.SingleOrDefault().RoleId,
                Email = user.Email,
                PayRollUser = user.PayRollUser,
                TimeZone = user.TimeZone,
                Phone = user.PhoneNumber
            };
                Employee em = user.Employee.SingleOrDefault();
                if (em != null) {
                    mode.DefaultJob = em.DefaultJob;
                    mode.SSN = em.SSN;
                    mode.Address = em.Address1;
                    mode.City = em.City;
                    mode.State = em.State;
                    mode.ZipCode = em.ZipCode;
                    
                }
                if (em != null) {
                    ViewBag.DefaultJobList = SelectHelper.JobToSelect(db, em.Job.ToList());
                }


            return View(mode);
        }
        [HttpPost]
        [Authorize]
        [CustomAuthorize]
        public async Task<ActionResult> GeneralSet(GeneralSetModel viewModel)
        {
            Dictionary<string, string> StaList = EnumHelper.GetEnumItemDesc(typeof(States));
            ViewBag.StatesList = new SelectList(StaList, "key", "value");
            //ViewBag.StatesList = new SelectList(EnumHelper.EnumListDic<States>("", ""), "value", "key");
            ViewBag.TimeZoneList = SelectHelper.TimeZoneToSelect(db);
            ViewBag.DefaultJob = new SelectList("", "Value", "Text");

            AppUser user = LoginUser;
            user.PayRollUser = viewModel.PayRollUser;
            user.PhoneNumber = viewModel.Phone;
            user.TimeZone = viewModel.TimeZone;
            Employee e = user.Employee.SingleOrDefault();
            if (e != null) {
                e.Phone = viewModel.Phone;
                e.TimeZone = viewModel.TimeZone;
                e.DefaultJob = viewModel.DefaultJob;
                e.SSN = viewModel.SSN;
                e.Address1 = viewModel.Address;
                e.City = viewModel.City;
                e.State = viewModel.State;
                e.ZipCode = viewModel.ZipCode;
            }
            var result = await UserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                db.Entry<Employee>(e).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GeneralSet");
            }
            AddErrors(result);
            return View(viewModel);
        }
        [Authorize]
        [CustomAuthorize]
        public ActionResult PasswordSet() {
            PasswordSetModel mode = new PasswordSetModel()
            {
                UserId = LoginUser.Id
            };

            return View(mode);
        }
        [HttpPost]
        [Authorize]
        [CustomAuthorize]
        public async Task<ActionResult> PasswordSet(PasswordSetModel viewModel)
        {
            var result = await UserManager.ChangePasswordAsync(viewModel.UserId, viewModel.Password, viewModel.NewPassword);
            if (result.Succeeded)
            {
                EnsureLoggedOut();
                return View(viewModel);
            }
            AddErrors(result);
            return View(viewModel);
        }
        private ActionResult RedirectToLocal(string returnUrl = "")
        {

            // If the return url starts with a slash "/" we assume it belongs to our site
            // so we will redirect to this "action"
            if (!returnUrl.IsNullOrWhiteSpace() && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            // If we cannot verify if the url is local to our host we redirect to a default location
            string RoleId = SystemVariates.LoginRoleId;
            if (RoleId == "R00")
                return RedirectToAction("UserControl", "System");
            else
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
            cookie["RoleId"] = user.Roles.FirstOrDefault().RoleId;
            cookie["RoleName"] = RoleManager.FindById(user.Roles.FirstOrDefault().RoleId).Name;
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
            AccountForgotPasswordModel mode = new AccountForgotPasswordModel() { Email = Email };
            return View(mode);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResendEmail(string Email)
        {
                var user = await UserManager.FindByEmailAsync(Email);
                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                string Pcode = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            //code = HttpUtility.UrlEncode(code);
            if (UserManager.IsInRole(user.Id, "Admin"))
            {
                var callbackUrl = Url.Action("CompleteRegist", "Account", new { userId = user.Id, code = code, Nt = TimeHelper.ConvertDateTimeInt(DateTime.UtcNow) }, protocol: Request.Url.Scheme);
                string EmailBody = "Thank you for creating an PayRoll account. You need to complete the registration information and confirm your account, you'll have access to PayRoll system.";
                string EmailLink = "Please confirm your account complete regist by clicking.<a href=\"" + callbackUrl + "\">Click it! &raquo;</a>";
                string strbody = ReplaceText(EmailBody, EmailLink);
                await UserManager.SendEmailAsync(user.Id, "Confirm Your Account – Complete Registration Information (" + user.Company.CompanyName + ")", strbody);
            }
            else
            {
                var callbackUrl = Url.Action("UserConfirm", "Account", new { userId = user.Id, code = code, Nt = TimeHelper.ConvertDateTimeInt(DateTime.UtcNow) }, protocol: Request.Url.Scheme);
                string EmailBody = "Thank you for creating an PayRoll account. You need to confirm your account and setting your password, you'll have access to PayRoll system.";
                string EmailLink = "Please confirm your account complete regist by clicking.<a href=\"" + callbackUrl + "\">Click it! &raquo;</a>";
                string strbody = ReplaceText(EmailBody, EmailLink);
                await UserManager.SendEmailAsync(user.Id, "Confirm Your Account  (" + user.PayRollUser + ")", strbody);
            }


            ViewBag.ReceiveEmail = user.Email;
            return View("WaitEmail");
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> UserConfirm(string userId, string code, string Nt)
        {
            AppUser user = UserManager.FindById(userId);
            double timeInt = Convert.ToDouble(Nt);
            if (timeInt + 3 * 3600 <= TimeHelper.ConvertDateTimeInt(DateTime.UtcNow) || timeInt > TimeHelper.ConvertDateTimeInt(DateTime.UtcNow) || timeInt == 0)
            {
                return RedirectToAction("Error404", "Account", new { ErrorMessage = "Link Has Expired" });
            }
            EnsureLoggedOut();
            if (userId == null || code == null || user == null)
            {
                return RedirectToLocal();
            }
            if (await UserManager.IsEmailConfirmedAsync(userId))
            {
                return RedirectToLocal();
            }
            UserConfirmModel mode = new UserConfirmModel()
            {
                Code = code,
                Id = userId
            };
            ViewBag.UserName = user.PayRollUser;
            return View(mode);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> UserConfirm(UserConfirmModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            var user = UserManager.FindById(viewModel.Id);
            if (user != null)
            {
                IdentityResult result = UserManager.ConfirmEmail(viewModel.Id, viewModel.Code);
                if (result.Succeeded)
                {
                    //await UserManager.ResetPasswordAsync(user.Id, viewModel.code, viewModel.Password);
                    var resultPW = UserManager.ChangePassword(user.Id, "Pay123456", viewModel.Password);
                    if (resultPW.Succeeded)
                    {
                        Employee e = user.Employee.SingleOrDefault();
                        e.F124 = 0;
                        db.SaveChanges();
                        await ApplicationSignInManager.PasswordSignInAsync(user.UserName, viewModel.Password, true, shouldLockout: false);
                        SignCookieAsync(user);
                        return RedirectToLocal();
                    }
                    else {
                        AddErrors(resultPW);
                        return View(viewModel);
                    };
                }
                else
                {
                    AddErrors(result);
                    return View(viewModel);
                }

            }
            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> CompleteRegist(string userId, string code,string Nt)
        {
            double timeInt = Convert.ToDouble(Nt);
            if (timeInt + 3 * 3600 <= TimeHelper.ConvertDateTimeInt(DateTime.UtcNow) || timeInt > TimeHelper.ConvertDateTimeInt(DateTime.UtcNow) || timeInt == 0)
            {
                return RedirectToAction("Error404", "Account", new { ErrorMessage = "Link Has Expired" });
            }
            EnsureLoggedOut();
            //EnsureLoggedOut();
            Dictionary<string, string> StaList = EnumHelper.GetEnumItemDesc(typeof(States));
            ViewBag.StatesList = new SelectList(StaList, "key", "value");
            //Dictionary<string, object> StaList = EnumHelper.EnumListDic<States>("", "");
            //ViewBag.StatesList = new SelectList(StaList, "value", "key");
            Dictionary<string, object> RoundTo = EnumHelper.EnumListDic<RoundTo>("", "");
            ViewBag.RoundTo = new SelectList(RoundTo, "value", "key");
            ViewBag.TimeZone = SelectHelper.TimeZoneToSelect(db);

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
                user.PayRollUser = viewModel.FName + " " + viewModel.LName;
                user.UserName = viewModel.Email;
                user.Company.ContactName = viewModel.FName + " " + viewModel.LName;
                user.LastLoginDate = DateTime.UtcNow;
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
                        Customer cus = new Customer()
                        {
                            Id = Guid.NewGuid(),
                            CustomerName = "Default Customer",
                            AddDate = DateTime.Now,
                            CompanyId = user.CompanyId
                        };
                        db.Customer.Add(cus);
                        Job job = new Job() { JobName = "Default Job", status = "0", CompanyId = user.CompanyId, Customer = new List<Customer>() };
                        db.Job.Add(job);
                        job.Customer.Add(cus);
                        db.SaveChanges();
                        Employee em = new Employee() {
                            EmployeeId = Guid.NewGuid(),
                            Email = user.Email,
                            Address1 = user.Company.Address1,
                            Address2 = user.Company.Address2,
                            State =user.Company.State,
                            City = user.Company.City,
                            DefaultJob =job.JobId,
                            TimeZone = user.TimeZone,
                            ZipCode = user.Company.Zip,
                            Phone = viewModel.Telephone,
                            FName = viewModel.FName,
                            LName = viewModel.LName,
                            SSN = viewModel.SSN,
                            UserRole = "Admin",
                            F99 = 0,
                            F106 = "Single",
                            F114 = 1,
                            F115 = 1,
                            F116 = 1,
                            F117 = 1,
                            F118 = 1,
                            F119 = 1,
                            F124 = 0,
                            F103 = DateTime.UtcNow,
                            F104 = DateTime.UtcNow,
                            F105 = DateTime.UtcNow,
                            CompanyId = user.CompanyId, 
                            AppUserId = user.Id,
                            Job = new List<Job>()

                    };
                        db.Employee.Add(em);
                        em.Job.Add(job);

                        Company c = db.Company.Where(t => t.CompanyId == user.CompanyId).SingleOrDefault();
                        if (c != null) {
                            c.ContactName = user.PayRollUser;
                            c.Status = "0";
                        }
                        db.Entry<Company>(c).State = System.Data.Entity.EntityState.Modified;

                        T100 t100 = new T100
                        {
                            Id = Guid.NewGuid(),
                            BankName = "My Bank",
                            BankInfo1 = "Bank Road",
                            TransitCode = "67-76890",
                            BankRouteNo = "123456789",
                            BankAccountNo = "0123456789",
                            StartCheckNo = 100,
                            CurrentCheckNo = 1000,
                            CheckWidth = 0,
                            CheckHeight = 0,
                            OffsetLeft = 0,
                            OffsetRight = 0,
                            OffsetUp = 0,
                            OffsetDown = 0,
                            Logo = "",
                            Signature = "",
                            Company1 = c.CompanyName,
                            Company2 = c.Address1,
                            Company3 = c.City + ", " + c.State + " " + c.Zip,
                            Company4 = "(111) 111-1111",
                            BlankBankStock = false,
                            ExField1 = "Bottom",
                            ExField2 = false,
                            ExField3 = "",
                            nodisplaymicr = false,
                            CompanyId = c.CompanyId
                        };
                        db.T100.Add(t100);
                        db.SaveChanges();
                        //await SignInAsync(user, true);
                        await ApplicationSignInManager.PasswordSignInAsync(user.UserName, viewModel.Password, true, shouldLockout: false);
                        RollPayInitialize(db,user.CompanyId);
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
        [HttpPost]
        public async Task<ActionResult> SemdEmail(string Id)
        {
            AppUser user = UserManager.FindById(Id);
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = Url.Action("UserConfirm", "Account", new { userId = user.Id, code = code, Nt = TimeHelper.ConvertDateTimeInt(DateTime.UtcNow) }, protocol: Request.Url.Scheme);
            string EmailBody = "Thank you for creating an PayRoll account. You need to confirm your account and setting your password, you'll have access to PayRoll system.";
            string EmailLink = "Please confirm your account complete regist by clicking.<a href=\"" + callbackUrl + "\">Click it! &raquo;</a>";
            string strbody = ReplaceText(EmailBody, EmailLink);
            await UserManager.SendEmailAsync(user.Id, "Confirm Your Account  (" + user.PayRollUser + ")", strbody);
            return Json(new { code = 1, message = "success" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckSetup() {
            AppUser user = LoginUser;
            T100 model = db.T100.Where(t => t.CompanyId == user.CompanyId).SingleOrDefault();
            int CurrentCheckNo = 0;
            var MaxList = db.T105.Where(t => t.CompanyId == user.CompanyId).GroupBy(t => t.Id).Select(t => t.Max(x => x.CheckNo)).ToList();
            foreach(var m in MaxList)
            {
                CurrentCheckNo = m;
            };
            ViewBag.CurrentCheckNo = CurrentCheckNo;
            return View(model);
        }
        [HttpPost]
        public ActionResult CheckSetup(T100 t)
        {
            if (!ModelState.IsValid)
                return View(t);
            T100 NewT = db.T100.Find(t.Id);
            NewT.BankName = t.BankName;
            NewT.BankInfo1 = t.BankInfo1;
            NewT.BankInfo2 = t.BankInfo2;
            NewT.BankInfo3 = t.BankInfo3;
            NewT.TransitCode = t.TransitCode;
            NewT.BankRouteNo = t.BankRouteNo;
            NewT.BankAccountNo = t.BankAccountNo;
            NewT.BlankBankStock = t.BlankBankStock;
            NewT.CurrentCheckNo = t.CurrentCheckNo;
            NewT.OffsetLeft = t.OffsetLeft;
            NewT.OffsetRight = t.OffsetRight;
            NewT.OffsetUp = t.OffsetUp;
            NewT.OffsetDown = t.OffsetDown;
            NewT.ExField1 = t.ExField1;
            NewT.ExField2 = t.ExField2;
            NewT.Logo = t.Logo;
            NewT.Signature = t.Signature;
            NewT.Company1 = t.Company1;
            NewT.Company2 = t.Company2;
            NewT.Company3 = t.Company3;
            NewT.Company4 = t.Company4;
            db.Entry(NewT).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("CheckSetup");
        }
        private void RollPayInitialize(AppIdentityDbContext Context, int CompanyId)
        {
            var t102 = db.T201.Where(t => t.CompanyId == CompanyId).ToList();
            if (t102.Count != 0) {
                return;
            }
            Context.T102.Add(new T102 { Id = Guid.NewGuid(), ItemId = 1, CodeMap = "F1231", Description = "Health Insurance", Enabled=true, AnnualLimit = 1000, CompanyId = CompanyId, Type = 1 });
            Context.T102.Add(new T102 { Id = Guid.NewGuid(), ItemId = 2, CodeMap = "F1232", Description = "401K", Enabled = true, AnnualLimit = 1000, CompanyId = CompanyId, Type = 1 });
            Context.T102.Add(new T102 { Id = Guid.NewGuid(), ItemId = 3, CodeMap = "F1233", Description = "SDI (State Disablility)", Enabled = true, AnnualLimit = 0, CompanyId = CompanyId, Type = 2 });
            Context.T102.Add(new T102 { Id = Guid.NewGuid(), ItemId = 4, CodeMap = "F1234", Description = "Changeable Deduction", AnnualLimit = 955.85, CompanyId = CompanyId, Type = 2 });
            Context.T102.Add(new T102 { Id = Guid.NewGuid(), ItemId = 5, CodeMap = "F1235", Description = "Changeable Deduction", AnnualLimit = 0, CompanyId = CompanyId, Type = 2 });
            Context.T102.Add(new T102 { Id = Guid.NewGuid(), ItemId = 6, CodeMap = "F1236", Description = "Changeable Deduction", AnnualLimit = 0, CompanyId = CompanyId, Type = 2 });
            Context.T102.Add(new T102 { Id = Guid.NewGuid(), ItemId = 7, CodeMap = "F1237", Description = "Changeable Deduction", AnnualLimit = 120, CompanyId = CompanyId, Type = 2 });

            Context.T201.Add(new Models.T201 { Id = Guid.NewGuid(), Ord = 1, CodeMap = "F102", Description = "Yealy Salary", Enabled = true, Type = 1, CompanyId = CompanyId });
            Context.T201.Add(new Models.T201 { Id = Guid.NewGuid(), Ord = 2, CodeMap = "F100", Description = "Regular Hourly Pay", Enabled = true, Type = 1, CompanyId = CompanyId });
            Context.T201.Add(new Models.T201 { Id = Guid.NewGuid(), Ord = 3, CodeMap = "F101", Description = "Overtime Hourly Pay", Enabled = true, Type = 1, CompanyId = CompanyId });
            Context.T201.Add(new Models.T201 { Id = Guid.NewGuid(), Ord = 4, CodeMap = "F1002", Description = "Double Hourly Pay", Enabled = true, Type = 1, CompanyId = CompanyId });
            Context.T201.Add(new Models.T201 { Id = Guid.NewGuid(), Ord = 5, CodeMap = "SickRate", Description = "Holiday Add", Enabled = false, Type = 1, CompanyId = CompanyId });
            Context.T201.Add(new Models.T201 { Id = Guid.NewGuid(), Ord = 6, CodeMap = "VacationRate", Description = "Night Shift Add", Enabled = false, Type = 1, CompanyId = CompanyId });
            Context.T201.Add(new Models.T201 { Id = Guid.NewGuid(), Ord = 7, CodeMap = "S1251", Description = "Bonus", Enabled = false, Type = 2, CompanyId = CompanyId });
            Context.T201.Add(new Models.T201 { Id = Guid.NewGuid(), Ord = 8, CodeMap = "S1252", Description = "Director Fee", Enabled = false, Type = 2, CompanyId = CompanyId });
            Context.T201.Add(new Models.T201 { Id = Guid.NewGuid(), Ord = 9, CodeMap = "S1253", Description = "Tips", Enabled = false, Type = 2, CompanyId = CompanyId });
            Context.T201.Add(new Models.T201 { Id = Guid.NewGuid(), Ord = 10, CodeMap = "S1254", Description = "Wage1", Enabled = false, Type = 2, CompanyId = CompanyId });
            Context.T201.Add(new Models.T201 { Id = Guid.NewGuid(), Ord = 11, CodeMap = "S1255", Description = "Wage1", Enabled = false, Type = 2, CompanyId = CompanyId });
            Context.SaveChanges();
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
                return HttpContext.GetOwinContext().Get<AppRoleManager>();
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
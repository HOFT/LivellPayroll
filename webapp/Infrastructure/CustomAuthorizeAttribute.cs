using LivellPayRoll.App_Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.Linq;

namespace LivellPayRoll.Infrastructure
{
    public class CustomAuthorizeAttribute: AuthorizeAttribute
    {
        private string[] AuthRoles { get; set; }
        //请求授权是调用
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            string roles = RoleViewHelper.GetActionRoles(actionName,controllerName);
            if (!string.IsNullOrWhiteSpace(roles))
            {
                this.AuthRoles = roles.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                this.AuthRoles = new string[] { };
            }

            base.OnAuthorization(filterContext);

            if (filterContext.HttpContext.Response.StatusCode == 403)
            {
                filterContext.Result = new RedirectResult("/Error/Error403");
            }
        }

        //自定义权限检查
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            AppIdentityDbContext db = DbContextFactory.DbCon();
            if (httpContext == null)
            {
                throw new ArgumentNullException("HttpContext");
            }
            if (AuthRoles == null || AuthRoles.Length == 0)
            {
                return true;
            }
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                //httpContext.Response.StatusCode = 403;
                return false;
            }
            //获取用户角色
            var user = db.Users.Where(u => u.UserName == httpContext.User.Identity.Name).SingleOrDefault();
            var useRoles = user.Roles.Select(r => r.RoleId).ToList();
            //验证用户角色是否属于AuthRoles
            for (int i = 0; i < AuthRoles.Length; i++)
            {
                if (useRoles.Contains(AuthRoles[i]))
                {
                    return true;
                }
            }
            httpContext.Response.StatusCode = 403;
            return false;
        }

    }
}
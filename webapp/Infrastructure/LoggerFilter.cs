using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace LivellPayRoll.Infrastructure
{
    public class LoggerFilter : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //执行action后执行这个方法 比如做操作日志  
        }
        public void OnActionExecuting(ActionExecutingContext filterContext)
        { 
            //执行action前执行这个方法,比如做身份验证  
            //如果存在身份信息
            if (!HttpContext.Current.User.Identity.IsAuthenticated/*|| HttpContext.Current.Request.Cookies["LoginInfo"] == null*/)
            {
                ContentResult Content = new ContentResult();
                Content.Content = string.Format("<script type='text/javascript'>alert('请先登录！');window.location.href='{0}';</script>", FormsAuthentication.LoginUrl);
                filterContext.Result = Content;
            }
        }
    }
}
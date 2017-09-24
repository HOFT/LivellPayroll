#region Using

using LivellPayRoll.Infrastructure;
using System.Web.Mvc;

#endregion

namespace LivellPayRoll
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //增加角色过滤
            //filters.Add(new CustomAuthorizeAttribute());
        }
    }
}
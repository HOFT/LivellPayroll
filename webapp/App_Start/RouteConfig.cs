#region Using

using System.Web.Mvc;
using System.Web.Routing;

#endregion

namespace LivellPayRoll
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.LowercaseUrls = true;
            routes.MapRoute("Default", "{controller}/{action}/{id}", new
            {
                controller = "Account",
                action = "Login",
                id = UrlParameter.Optional
            }).RouteHandler = new DashRouteHandler();
        }
    }
}
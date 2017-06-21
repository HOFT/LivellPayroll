#region Using
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

#endregion

namespace LivellPayRoll
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //Database.SetInitializer<LivellPayRollDbContext>(new CreateDatabaseIfNotExists<LivellPayRollDbContext>());
            ////数据化数据库数据
            //Database.SetInitializer<LivellPayRollDbContext>(new LivellPayRollInitializer());
            AreaRegistration.RegisterAllAreas();
            //IdentityConfig.RegisterIdentities();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
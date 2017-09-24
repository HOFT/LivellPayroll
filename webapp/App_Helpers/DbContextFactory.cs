using LivellPayRoll.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LivellPayRoll.App_Helpers
{
    public class DbContextFactory
    {
        public static AppIdentityDbContext DbCon()
        {
            AppIdentityDbContext dbContext = HttpContext.Current.Items["AppIdentityDbContext"] as AppIdentityDbContext;
            if (dbContext == null)
            {
                dbContext = AppIdentityDbContext.Create();
                HttpContext.Current.Items["AppIdentityDbContext"] = dbContext;
            }
            return dbContext;
        }
    }
}
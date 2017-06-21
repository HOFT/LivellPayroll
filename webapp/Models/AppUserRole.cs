using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Models
{
    public class AppUserRole:IdentityUserRole
    {
        public AppUserRole() : base() { }
    }
}
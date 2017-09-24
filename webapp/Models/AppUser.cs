using LivellPayRoll.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace LivellPayRoll.Models
{
    public class AppUser:IdentityUser
    {
        // 这里将放置附加属性
        public AppUser() : base() { }
        [StringLength(8)]
        [Required]
        public string TimeZone { get; set; }
        [StringLength(256)]
        public string PayRollUser { get; set; }
        public DateTime LastLoginDate { get; set; }
        public virtual ICollection<Employee> Employee { get; set; }
        public virtual ICollection<TimeSheetLog> TimeSheetLog { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(AppUserManager userManager)
        {
            var userIdentity = await userManager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
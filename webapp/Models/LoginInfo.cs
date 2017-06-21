using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Models
{
    public class LoginInfo
    {
        public string UserId { set; get; }
        public string UserName { set; get; }
        public string Email { set; get; }
        public string RoleId { set; get; }
        public int CompanyId { set; get; }
    }
}
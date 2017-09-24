using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LivellPayRoll.Controllers
{
    [Authorize]
    public class ErrorController : Controller
    {
        [AllowAnonymous]
        public ActionResult Error403()
        {
            return View();
        }
    }
}
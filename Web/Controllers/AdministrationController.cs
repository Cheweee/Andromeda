using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Andromeda.Web.Controllers
{
    public class AdministrationController : Controller
    {
        [HttpGet]
        public ActionResult Users()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Roles()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Rights()
        {
            return View();
        }
    }
}
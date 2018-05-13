using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Andromeda.Web.Controllers
{
    public class CirriculumDevelopmentController : Controller
    {
        [HttpGet]
        public ActionResult Seats()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Workers()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AreasOfTraning()
        {
            return View();
        }
        [HttpGet]
        public ActionResult WorkingCirriculums()
        {
            return View();
        }
    }
}
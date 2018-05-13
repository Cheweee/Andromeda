using Andromeda.Core.Logs;
using Andromeda.Core.Managers;
using Andromeda.ViewModels.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Andromeda.Web.Controllers
{
    public class BaseController : Controller
    {
        private string loginUrl = "/Base/Login";
        private string homeUrl = "/Base/Home";

        [HttpGet]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(homeUrl);
            }
            return View();
        }
        [HttpPost]
        public JsonResult Login(string login, string password, bool remember)
        {
            var data = UserManager.Login(login, password, remember);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public RedirectResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect(loginUrl);
        }
        [HttpGet]
        public ActionResult Home()
        {
            return OnPageLoad();
        }
        [HttpPost]
        public JsonResult PageLoaded()
        {
            var data = UserManager.LoadUserAccessiblePages();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Help()
        {
            return OnPageLoad();
        }
        [HttpGet]
        public ActionResult Settings()
        {
            return OnPageLoad();
        }
        [HttpGet]
        public ActionResult Account()
        {
            return OnPageLoad();
        }

        protected ActionResult OnPageLoad()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect(loginUrl);
            }

            return View();
        }
    }
}
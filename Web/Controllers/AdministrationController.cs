using Andromeda.Core.Managers;
using Andromeda.Models.Administration;
using Andromeda.ViewModels.Client;
using Andromeda.ViewModels.Server;
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
        #region Roles
        [HttpGet]
        public ActionResult Roles()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetRoles(EntitiesViewModel model)
        {
            var data = BaseEntityManager.GetEntities<Role, RoleViewModel>(
                model.Page,
                model.Limit,
                model.Order,
                model.IsAscending(),
                o=> o.Name.ToLower().Contains((model.Search ?? string.Empty).ToLower()),
                o=> new RoleViewModel { Id = o.Id, Name = o.Name });

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetRoleRights(EntitiesViewModel model)
        {
            var data = BaseEntityManager.GetEntities<Right, RightViewModel>(
                o=> o.Name.ToLower().Contains((model.Search ?? string.Empty).ToLower()),
                o=> new RightViewModel { Id = o.Id, Name = o.Name });

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddRole(Role entity)
        {
            entity.Id = Guid.NewGuid();
            var data = BaseEntityManager.AddEntity(entity);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ModifyRole(Role entity)
        {
            var data = BaseEntityManager.ModifyEntity(entity);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteRoles(List<Role> entities)
        {
            var data = BaseEntityManager.DeleteEntities(entities);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Rights
        [HttpGet]
        public ActionResult Rights()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetRights(EntitiesViewModel model)
        {
            var data = BaseEntityManager.GetEntities<Right, RightViewModel>(
                model.Page,
                model.Limit,
                model.Order,
                model.IsAscending(),
                o => o.Name.ToLower().Contains((model.Search ?? string.Empty).ToLower()),
                o => new RightViewModel
                {
                    Id = o.Id,
                    Name = o.Name
                });

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
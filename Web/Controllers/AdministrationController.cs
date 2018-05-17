using Andromeda.Core.Managers;
using Andromeda.Models.Administration;
using Andromeda.Models.References;
using Andromeda.Models.RelationEntities;
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
        #region Users
        [HttpGet]
        public ActionResult Users()
        {
            return View();
        }
        [HttpGet]
        public new ActionResult User()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetUsers(EntitiesViewModel model)
        {
            var data = UserManager.GetUsers(model.Page, model.Limit, model.Order, model.IsAscending(), model.Search);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetUser(Guid id)
        {
            var data = UserManager.GetUser(id);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetNotUserAcademicDegrees(EntitiesViewModel model)
        {
            var data = UserManager.GetNotUserAcademicDegrees(model.SearchId, model.Search);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetUserAcademicDegrees(EntitiesViewModel model)
        {
            var data = UserManager.GetUserAcademicDegrees(model.SearchId);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAcademicTitles(EntitiesViewModel model)
        {
            var data = BaseEntityManager.GetEntities<AcademicTitle, AcademicTitleViewModel>(
                o => o.ShortName.ToLower().Contains((model.Search ?? string.Empty).ToLower()) ||
                o.Name.ToLower().Contains((model.Search ?? string.Empty).ToLower()),
                o => new AcademicTitleViewModel
                {
                    Id = o.Id,
                    Name = o.Name,
                    ShortName = o.ShortName
                });

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddUser(User entity)
        {
            var data = BaseEntityManager.AddEntity(entity);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ModifyUser(User entity)
        {
            var data = BaseEntityManager.ModifyEntity(entity);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteUsers(List<User> entities)
        {
            var data = BaseEntityManager.DeleteEntities(entities);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
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
        public JsonResult GetRole(Guid id)
        {
            var data = BaseEntityManager.GetEntity<Role, RoleViewModel>(o => o.Id == id,
                o => new RoleViewModel { Id = o.Id, Name = o.Name, CanTeach = o.CanTeach });

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetNotRoleRights(EntitiesViewModel model)
        {
            var data = RoleManager.GetNotRoleRights(model.SearchId, model.Search);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetRoleRights(EntitiesViewModel model)
        {
            var data = BaseEntityManager.GetEntitiesWithJoin<RightRole, Right, Guid, RightViewModel>(
                o => o.RoleId == model.SearchId,
                rr => rr.RightId,
                r => r.Id,
                (r, rr) => new RightViewModel { Id = r.Id, Name = r.Name });

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveRoleRights(ChangeEntitiesViewModel<Right> model)
        {
            var data = RoleManager.SaveRoleRights(model.NewId, model.Entities.Select(o => o.Id).ToList());

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
        [HttpPost]
        public JsonResult IsUserSystemAdmin()
        {
            var data = UserManager.IsUserSystemAdmin();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
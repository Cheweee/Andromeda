﻿using Andromeda.Core.Managers;
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
        #region Users
        [HttpGet]
        public ActionResult Users()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetUsers(EntitiesViewModel model)
        {
            var data = UserManager.GetUsers(model.Page, model.Limit, model.Order, model.IsAscending(), model.Search);
                //BaseEntityManager.GetEntities<User, UserViewModel>(model.Page, model.Limit, model.Order, model.IsAscending(),
                //o => o.UserName.ToLower().Contains((model.Search ?? string.Empty)) ||
                //o.LastName.ToLower().Contains((model.Search ?? string.Empty)) ||
                //o.Login.ToLower().Contains((model.Search ?? string.Empty)) ||
                //o.Patronimyc.ToLower().Contains((model.Search ?? string.Empty)),
                //o=> new UserViewModel {
                //    Id = o.Id,
                //    LastName = o.LastName,
                //    Login = o.Login,
                //    Name = o.UserName,
                //    Patronymic = o.Patronimyc ?? string.Empty
                //});

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetUser(Guid id)
        {
            var data = BaseEntityManager.GetEntity<User, UserViewModel>(o => o.Id == id,
                o => new UserViewModel
                {
                    Id = o.Id,
                    LastName = o.LastName,
                    Login = o.Login,
                    Name = o.UserName,
                    Patronymic = o.Patronimyc
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
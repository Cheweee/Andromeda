using Andromeda.Core.Managers;
using Andromeda.Models.Entities;
using Andromeda.Models.References;
using Andromeda.ViewModels.Client;
using Andromeda.ViewModels.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Andromeda.Web.Controllers
{
    public class CirriculumDevelopmentController : Controller
    {
        #region Seats
        [HttpGet]
        public ActionResult Seats()
        {
            return View();
        }
        #endregion
        #region Workers
        [HttpGet]
        public ActionResult Workers()
        {
            return View();
        }
        #endregion
        #region Areas of training
        [HttpGet]
        public ActionResult AreasOfTraining()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetAreasOfTraining(EntitiesViewModel model)
        {
            var data = AreaOfTrainingManager.GetAreasOfTraining(model.Page, model.Limit, model.Order, model.IsAscending(), model.Search);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAreaOfTraining(Guid id)
        {
            var data = AreaOfTrainingManager.GetAreaOfTraining(id);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAreaOfTrainingDepartments(EntitiesViewModel model)
        {
            var data = BaseEntityManager.GetEntities<Department, DepartmentViewModel>(
                o => !o.IsFaculty && (o.Name.ToLower().Contains((model.Search ?? string.Empty).ToLower()) ||
            o.ShortName.ToLower().Contains((model.Search ?? string.Empty).ToLower())),
            o => new DepartmentViewModel
            {
                Id = o.Id,
                Name = o.Name
            }); ;

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAreaOfTrainingLevelsOfHigherEducation(EntitiesViewModel model)
        {
            var data = BaseEntityManager.GetEntities<LevelOfHigherEducation, LevelOfHigherEducationViewModel>(
                o => o.Name.ToLower().Contains((model.Search ?? string.Empty).ToLower()),
            o => new LevelOfHigherEducationViewModel { Name = o.Name });

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddAreaOfTraining(AreaOfTraining entity)
        {
            entity.Id = Guid.NewGuid();
            var data = BaseEntityManager.AddEntity(entity);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ModifyAreaOfTraining(AreaOfTraining entity)
        {
            var data = BaseEntityManager.ModifyEntity(entity);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteAreasOfTrainings(List<AreaOfTraining> entities)
        {
            var data = BaseEntityManager.DeleteEntities(entities);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Working cirriculums
        [HttpGet]
        public ActionResult WorkingCirriculums()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetWorkingCirriculums(EntitiesViewModel model)
        {
            var data = WorkingCirriculumManager.GetWorkingCirriculums(model.Page, model.Limit, model.Order, model.IsAscending(), model.Search);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetWorkingCirriculum(Guid id)
        {
            var data = WorkingCirriculumManager.GetWorkingCirriculum(id);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetWorkingCirriculumDepartments(EntitiesViewModel model)
        {
            var data = BaseEntityManager.GetEntities<Department, DepartmentViewModel>(
                o => !o.IsFaculty && (o.Name.ToLower().Contains((model.Search ?? string.Empty).ToLower()) ||
            o.ShortName.ToLower().Contains((model.Search ?? string.Empty).ToLower())),
            o => new DepartmentViewModel
            {
                Id = o.Id,
                Name = o.Name
            }); ;

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetWorkingCirriculumAreaOfTrainings(EntitiesViewModel model)
        {
            var data = BaseEntityManager.GetEntities<AreaOfTraining, AreaOfTrainingViewModel>(
                o => o.Name.ToLower().Contains((model.Search ?? string.Empty).ToLower()),
            o => new AreaOfTrainingViewModel { Id = o.Id, Name = o.Name });

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetWorkingCirriculumTypesOfEducation(EntitiesViewModel model)
        {
            var data = BaseEntityManager.GetEntities<TypeOfEducation, string>(
                o => o.Name.ToLower().Contains((model.Search ?? string.Empty).ToLower()),
            o => o.Name);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddWorkingCirriculum(WorkingCirriculum entity)
        {
            entity.Id = Guid.NewGuid();
            var data = BaseEntityManager.AddEntity(entity);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ModifyWorkingCirriculum(WorkingCirriculum entity)
        {
            var data = BaseEntityManager.ModifyEntity(entity);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteWorkingCirriculums(List<WorkingCirriculum> entities)
        {
            var data = BaseEntityManager.DeleteEntities(entities);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
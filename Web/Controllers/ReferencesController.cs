using Andromeda.Core.Managers;
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
    public class ReferencesController : Controller
    {
        #region Course titles
        [HttpGet]
        public ActionResult CourseTitles()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetCourseTitles(EntitiesViewModel model)
        {

            var data = CourseTitleManager.GetCourseTitles(model.Page,
                model.Limit,
                model.Order,
                model.IsAscending(),
                model.Search);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetCourseTitle(Guid id)
        {
            var data = CourseTitleManager.GetCourseTitle(id);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddCourseTitle(CourseTitle entity)
        {
            entity.Id = Guid.NewGuid();
            var data = BaseEntityManager.AddEntity(entity);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ModifyCourseTitle(CourseTitle entity)
        {
            var data = BaseEntityManager.ModifyEntity(entity);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetCourseTitleDepartments(EntitiesViewModel model)
        {
            var data = BaseEntityManager.GetEntities<Department, DepartmentViewModel>(
                o => o.Name.ToLower().Contains((model.Search ?? string.Empty).ToLower()) ||
            o.ShortName.ToLower().Contains((model.Search ?? string.Empty).ToLower()),
            o => new DepartmentViewModel
            {
                Id = o.Id,
                Name = o.Name
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteCourseTitles(List<CourseTitle> entities)
        {
            var data = BaseEntityManager.DeleteEntities(entities);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Departments
        [HttpGet]
        public ActionResult Departments()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetDepartments(EntitiesViewModel model)
        {

            var data = DepartmentManager.GetDepartments(model.Page,
                model.Limit,
                model.Order,
                model.IsAscending(),
                model.Search);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetDepartment(Guid id)
        {
            var data = DepartmentManager.GetDepartment(id);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetDepartmentFaculties(EntitiesViewModel model)
        {
            var data = BaseEntityManager.GetEntities<Department, DepartmentViewModel>(
                o => o.IsFaculty && (o.Name.ToLower().Contains((model.Search ?? string.Empty).ToLower()) ||
            o.ShortName.ToLower().Contains((model.Search ?? string.Empty).ToLower())),
            o => new DepartmentViewModel
            {
                Id = o.Id,
                Name = o.Name
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult IsUserHeadOfDepartment()
        {
            var data = UserManager.IsUserHeadOfDepartment();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddDepartment(Department entity)
        {
            entity.Id = Guid.NewGuid();
            entity.IsFaculty = false;
            var data = BaseEntityManager.AddEntity(entity);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ModifyDepartment(Department entity)
        {
            var data = BaseEntityManager.ModifyEntity(entity);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteDepartments(List<Department> entities)
        {
            var data = BaseEntityManager.DeleteEntities(entities);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Faculties
        [HttpGet]
        public ActionResult Faculties()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetFaculties(EntitiesViewModel model)
        {
            var data = BaseEntityManager.GetEntities<Department, DepartmentViewModel>(
                model.Page,
                model.Limit,
                model.Order,
                model.IsAscending(),
                o=> o.IsFaculty && (o.Name.ToLower().Contains((model.Search ?? string.Empty).ToLower()) || o.ShortName.ToLower().Contains((model.Search ?? string.Empty).ToLower())),
                o=> new DepartmentViewModel {
                    Id = o.Id,
                    Name = o.Name,
                    ShortName = o.ShortName
                });

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetFaculty(Guid id)
        {
            var data = BaseEntityManager.GetEntity<Department, DepartmentViewModel>(
                o=> o.Id == id, o=> new DepartmentViewModel
                {
                    Id = o.Id,
                    Name = o.Name,
                    ShortName = o.ShortName
                });

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetNotFacultyDepartments(EntitiesViewModel model)
        {
            var data = BaseEntityManager.GetEntities<Department, DepartmentViewModel>(
                o => !o.IsFaculty && o.FacultyId == null && (o.Name.ToLower().Contains((model.Search ?? string.Empty).ToLower()) ||
            o.ShortName.ToLower().Contains((model.Search ?? string.Empty).ToLower())),
            o => new DepartmentViewModel
            {
                Id = o.Id,
                Name = o.Name,
                ShortName = o.ShortName
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetFacultyDepartments(EntitiesViewModel model)
        {
            var data = BaseEntityManager.GetEntities<Department, DepartmentViewModel>(
                o => !o.IsFaculty && o.FacultyId == model.SearchId && (o.Name.ToLower().Contains((model.Search ?? string.Empty).ToLower()) ||
            o.ShortName.ToLower().Contains((model.Search ?? string.Empty).ToLower())),
            o => new DepartmentViewModel
            {
                Id = o.Id,
                Name = o.Name,
                ShortName = o.ShortName
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveFacultyDepartments(ChangeEntitiesViewModel<Department> model)
        {
            var data = DepartmentManager.SaveFacultyDepartments(model.NewId, model.Entities.Select(o => o.Id).ToList());

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult IsUserDean()
        {
            var data = UserManager.IsUserDean();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddFaculty(Department entity)
        {
            entity.Id = Guid.NewGuid();
            entity.IsFaculty = true;
            var data = BaseEntityManager.AddEntity(entity);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ModifyFaculty(Department entity)
        {
            var data = BaseEntityManager.ModifyEntity(entity);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteFaculties(List<Department> entities)
        {
            var data = DepartmentManager.DeleteFaculties(entities);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region TypesOfProjects
        [HttpGet]
        public ActionResult TypesOfProjects()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetTypesOfProjects(EntitiesViewModel model)
        {
            var data = BaseEntityManager.GetEntities<TypeOfProject, TypeOfProjectsViewModel>(model.Page,
                model.Limit,
                model.Order,
                model.IsAscending(),
                o=> o.Name.ToLower().Contains((model.Search ?? string.Empty).ToLower()) ||
                o.ShortName.ToLower().Contains((model.Search ?? string.Empty).ToLower()),
                o=> new TypeOfProjectsViewModel {
                Id = o.Id,
                Name = o.Name,
                ShortName = o.ShortName
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetTypeOfProject(Guid id)
        {
            var data = BaseEntityManager.GetEntity<TypeOfProject, TypeOfProjectsViewModel>(o => o.Id == id, o => new TypeOfProjectsViewModel
            {
                Id = o.Id,
                Name = o.Name,
                ShortName = o.ShortName
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddTypeOfProjects(TypeOfProject entity)
        {
            entity.Id = Guid.NewGuid();
            var data = BaseEntityManager.AddEntity(entity);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ModifyTypeOfProjects(TypeOfProject entity)
        {
            var data = BaseEntityManager.ModifyEntity(entity);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteTypesOfProjects(List<TypeOfProject> entities)
        {
            var data = BaseEntityManager.DeleteEntities(entities);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        [HttpPost]
        public JsonResult CanUserEditReferences()
        {
            var data = UserManager.CanUserEditReferences();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
using Andromeda.Core.Logs;
using Andromeda.Models.Context;
using Andromeda.ViewModels.Server;
using Andromeda.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Andromeda.Models.References;

namespace Andromeda.Core.Managers
{
    public class DepartmentManager : BaseEntityManager
    {
        public static IViewModel GetDepartments(int page, int limit, string order, bool isAscending, string search)
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    EntitiesViewModel<DepartmentViewModel> result = new EntitiesViewModel<DepartmentViewModel>
                    {
                        Result = Result.Ok
                    };

                    var tempEntities = context.Departments.Where(o=> o.IsFaculty).Join(context.Departments.Where(o=> !o.IsFaculty).AsNoTracking(),
                        fo => fo.Id,
                        so => so.FacultyId,
                        (fo, so) => new DepartmentViewModel
                    {
                        FacultyId = fo.Id,
                        FacultyName = fo.Name ?? "Не привязана к факультету",
                        FacultyShortName = fo.ShortName ?? "Не привязана к факультету",
                        Id = so.Id,
                        Name = so.Name,
                        ShortName = so.ShortName,
                        Code = so.Code ?? 0
                    }).ToList();
                    var entities = isAscending ? tempEntities.Where(
                o => o.Name.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                o.ShortName.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                o.FacultyName.ToLower().Contains((search ?? string.Empty).ToLower()))
                    .OrderBy(order)
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToList() :
                     tempEntities.Where(
                o => o.Name.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                o.ShortName.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                o.FacultyName.ToLower().Contains((search ?? string.Empty).ToLower()))
                    .OrderByDescending(order)
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToList();

                    result.Total = context.Departments.Where(o => !o.IsFaculty).AsNoTracking().Count();
                    result.Entities = entities;
                    result.Page = page;
                    return result;
                }
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel GetFaculties(int page, int limit, string order, bool isAscending, string search)
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    EntitiesViewModel<DepartmentViewModel> result = new EntitiesViewModel<DepartmentViewModel>
                    {
                        Result = Result.Ok
                    };

                    result.Entities = GetEntities<Department, DepartmentViewModel>(context, page, limit, order, isAscending, 
                        o=> o.IsFaculty && (
                        o.Name.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                        o.ShortName.ToLower().Contains((search ?? string.Empty).ToLower())),
                        o=> new DepartmentViewModel
                        {
                            Id = o.Id,
                            Name = o.Name,
                            ShortName = o.ShortName
                        });
                    result.Total = context.Departments.Where(o => o.IsFaculty).AsNoTracking().Count();
                    result.Page = page;
                    return result;
                }
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel GetDepartment(Guid id)
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    EntityViewModel<DepartmentViewModel> result = new EntityViewModel<DepartmentViewModel>
                    {
                        Result = Result.Ok
                    };
                    result.Entity = context.Departments
                        .Where(o => o.Id == id)
                        .AsNoTracking()
                        .Select(o => new DepartmentViewModel
                        {
                            Id = o.Id,
                            Name = o.Name,
                            ShortName = o.ShortName,
                            FacultyId = context.Departments.Where(d => d.Id == o.FacultyId).Select(d => d.Id).FirstOrDefault(),
                            FacultyName = context.Departments.Where(d => d.Id == o.FacultyId).Select(d => d.Name).FirstOrDefault(),
                        })
                        .FirstOrDefault();

                    return result;
                }
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel SaveFacultyDepartments(Guid? facultyId, List<Guid> ids)
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    ResultViewModel result = new ResultViewModel
                    {
                        Result = Result.Ok
                    };

                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Configuration.ValidateOnSaveEnabled = false;

                    var departments = context.Departments.Where(o => ids.Contains(o.Id)).AsNoTracking().ToList();
                    foreach(var department in departments)
                    {
                        department.FacultyId = facultyId;
                        context.Entry(department).State = EntityState.Modified;
                    }
                    context.SaveChanges();

                    context.Configuration.AutoDetectChangesEnabled = true;
                    context.Configuration.ValidateOnSaveEnabled = true;

                    return result;
                }
            }
            catch(Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
    }
}

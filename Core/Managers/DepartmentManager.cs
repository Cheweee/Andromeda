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
                        FacultyName = fo.Name ?? string.Empty,
                        FacultyShortName = fo.ShortName ?? string.Empty,
                        Id = so.Id,
                        Name = so.Name,
                        ShortName = so.ShortName,
                        Code = so.Code ?? 0
                    }).ToList();
                    tempEntities.AddRange(context.Departments.Where(o => !o.IsFaculty && o.FacultyId == null).Select(o => new DepartmentViewModel
                    {
                        Code = o.Code ?? 0,
                        Id = o.Id,
                        Name = o.Name,
                        ShortName = o.ShortName
                    }));

                    var tempCollection = tempEntities.Where(
                o => o.Name.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                o.ShortName.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                o.FacultyName.ToLower().Contains((search ?? string.Empty).ToLower())).ToList();

                    result.Total = tempCollection.Count;

                    result.Entities = isAscending ? tempCollection
                    .OrderBy(order)
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToList() :
                     tempCollection
                    .OrderByDescending(order)
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToList();
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
        public static IViewModel DeleteFaculties(List<Department> entities)
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Configuration.ValidateOnSaveEnabled = false;

                    var entitiesIds = entities.Select(d => d.Id).ToList();
                    var facultyDepartments = context.Departments.Where(o => entitiesIds.Contains(o.FacultyId ?? Guid.Empty));
                    foreach(Department entity in facultyDepartments)
                    {
                        entity.FacultyId = null;
                        context.Entry(entity).State = EntityState.Modified;
                    }

                    foreach (Department entity in entities)
                    {
                        context.Entry(entity).State = EntityState.Deleted;
                    }
                    context.SaveChanges();

                    context.Configuration.AutoDetectChangesEnabled = true;
                    context.Configuration.ValidateOnSaveEnabled = true;
                }
                return new ResultViewModel { Result = Result.Ok };
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
    }
}

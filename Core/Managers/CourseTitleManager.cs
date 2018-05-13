using Andromeda.Core.Logs;
using Andromeda.Common.Extensions;
using Andromeda.Models.Context;
using Andromeda.ViewModels.Server;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Core.Managers
{
    public class CourseTitleManager
    {
        public static IViewModel GetCourseTitles(int page, int limit, string order, bool isAscending, string search)
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    EntitiesViewModel<CourseTitleViewModel> result = new EntitiesViewModel<CourseTitleViewModel>
                    {
                        Result = Result.Ok
                    };

                    var tempEntities = context.Departments.Join(context.CourseTitles.AsNoTracking(), fo => fo.Id, so => so.DepartmentId, (fo, so) => new CourseTitleViewModel
                    {
                        DepartmentId = fo.Id,
                        DepartmentName = fo.Name,
                        Id = so.Id,
                        Name = so.Name,
                        ShortName = so.ShortName
                    }).ToList();
                    var entities = isAscending ? tempEntities.Where(
                o => o.Name.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                o.ShortName.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                o.DepartmentName.ToLower().Contains((search ?? string.Empty).ToLower()))
                    .OrderBy(order)
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToList() :
                     tempEntities.Where(
                o => o.Name.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                o.ShortName.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                o.DepartmentName.ToLower().Contains((search ?? string.Empty).ToLower()))
                    .OrderByDescending(order)
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToList();

                    result.Total = context.CourseTitles.AsNoTracking().Count();
                    result.Entities = entities;
                    result.Page = page;
                    return result;
                }
            }
            catch(Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel GetCourseTitle(Guid id)
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    EntityViewModel<CourseTitleViewModel> result = new EntityViewModel<CourseTitleViewModel>
                    {
                        Result = Result.Ok
                    };
                    result.Entity = context.CourseTitles
                        .Where(o => o.Id == id)
                        .AsNoTracking()
                        .Select(o => new CourseTitleViewModel
                        {
                            Id = o.Id,
                            Name = o.Name,
                            ShortName = o.ShortName,
                            DepartmentId = context.Departments.Where(d => d.Id == o.DepartmentId).Select(d => d.Id).FirstOrDefault(),
                            DepartmentName = context.Departments.Where(d => d.Id == o.DepartmentId).Select(d => d.Name).FirstOrDefault(),
                        })
                        .FirstOrDefault();

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

using Andromeda.Core.Logs;
using Andromeda.Models.Context;
using Andromeda.Common.Extensions;
using Andromeda.ViewModels.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Andromeda.Core.Managers
{
    public class AreaOfTrainingManager
    {
        public static IViewModel GetAreasOfTraining(int page, int limit, string order, bool isAscending, string search)
        {
            try
            {
                using (DBContext context= DBContext.Create())
                {
                    EntitiesViewModel<AreaOfTrainingViewModel> data = new EntitiesViewModel<AreaOfTrainingViewModel>
                    {
                        Result = Result.Ok
                    };

                    var tempEntities = context.AreasOfTraining.AsNoTracking().Join(context.Departments.Where(o => !o.IsFaculty).AsNoTracking(),
                        fo => fo.DepartmentId,
                        so => so.Id,
                        (a, d) => new AreaOfTrainingViewModel
                        {
                            Id = a.Id,
                            Name = a.Name,
                            ShortName = a.ShortName,
                            Code = a.Code,
                            Directionaly = a.Directionaly,
                            LevelOfHigherEducationName = a.LevelOfHigherEducationName,
                            DepartmentId = d.Id,
                            DepartmentName = d.Name,
                            FacultyId = d.FacultyId,
                            FacultyName = context.Departments.Where(o => o.IsFaculty && o.Id == d.FacultyId).Select(o => o.Name).FirstOrDefault()
                        }).ToList();

                    var tempCollection = tempEntities.Where(
                o => o.Name.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                o.ShortName.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                o.Directionaly.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                o.LevelOfHigherEducationName.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                o.DepartmentName.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                o.FacultyName.ToLower().Contains((search ?? string.Empty).ToLower())
                ).ToList();

                    data.Total = tempCollection.Count;

                    data.Entities = isAscending ? tempCollection
                    .OrderBy(order)
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToList() :
                     tempCollection
                    .OrderByDescending(order)
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToList();
                    data.Page = page;
                    return data;

                }
            }
            catch(Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel GetAreaOfTraining(Guid id)
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    EntityViewModel<AreaOfTrainingViewModel> data = new EntityViewModel<AreaOfTrainingViewModel>
                    {
                        Result = Result.Ok
                    };

                    data.Entity = context.AreasOfTraining
                        .Where(o => o.Id == id)
                        .AsNoTracking()
                        .Select(o => new AreaOfTrainingViewModel
                        {
                            Id = o.Id,
                            Name = o.Name,
                            Code = o.Code,
                            ShortName = o.ShortName,
                            Directionaly = o.Directionaly,
                            LevelOfHigherEducationName = o.LevelOfHigherEducationName,
                            DepartmentId = context.Departments.Where(d => d.Id == o.DepartmentId).Select(d => d.Id).FirstOrDefault(),
                            DepartmentName = context.Departments.Where(d => d.Id == o.DepartmentId).Select(d => d.Name).FirstOrDefault(),
                        })
                        .FirstOrDefault();

                    return data;
                }
            }
            catch(Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
    }
}

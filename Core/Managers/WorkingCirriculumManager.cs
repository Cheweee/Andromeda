using Andromeda.Core.Logs;
using Andromeda.Models.Context;
using Andromeda.Common.Extensions;
using Andromeda.ViewModels.Server;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;

namespace Andromeda.Core.Managers
{
    public class WorkingCirriculumManager
    {
        public static IViewModel GetWorkingCirriculums(int page, int limit, string order, bool isAscending, string search)
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    EntitiesViewModel<WorkingCirriculumViewModel> data = new EntitiesViewModel<WorkingCirriculumViewModel>
                    {
                        Result = Result.Ok
                    };

                    var tempEntities = context.Departments.Where(o=> !o.IsFaculty).Join(context.WorkingCirriculums.AsNoTracking(), 
                        d=> d.Id,
                        w=> w.DepartmentId,
                        (d, w) => new WorkingCirriculumViewModel
                        {
                            Id = w.Id,
                            AreaOfTrainingId = w.AreaOfTrainingId,
                            StartTraining = w.StartTraining,
                            TrainingPeriod = w.TrainingPeriod,
                            TypeOfEducationName = w.TypeOfEducationName,
                            AreaOfTrainingName = context.AreasOfTraining.Where(a => a.Id == w.AreaOfTrainingId).Select(a => a.Name).FirstOrDefault(),
                            DepartmentId = d.Id,
                            DepartmentName = d.Name,
                            EducationalStandart = w.EducationalStandart,
                            FacultyId = context.Departments.Where(f => f.IsFaculty && f.Id == d.FacultyId).Select(f => f.Id).FirstOrDefault(),
                            FacultyName = context.Departments.Where(f => f.IsFaculty && f.Id == d.FacultyId).Select(f => f.Name).FirstOrDefault()
                        }).ToList();
                    tempEntities = tempEntities.Where(o => o.AreaOfTrainingName.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                    o.DepartmentName.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                    o.EducationalStandart.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                    o.FacultyName.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                    o.StartTraining.ToString().ToLower().Contains((search ?? string.Empty).ToLower()) ||
                    o.TrainingPeriod.ToString().ToLower().Contains((search ?? string.Empty).ToLower()) ||
                    o.TypeOfEducationName.ToLower().Contains((search ?? string.Empty).ToLower())).ToList();

                    data.Total = tempEntities.Count;

                    data.Entities = isAscending ? tempEntities
                    .OrderBy(order)
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToList() :
                     tempEntities
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
        public static IViewModel GetWorkingCirriculum(Guid id)
        {
            try
            {
                using (DBContext context= new DBContext())
                {
                    EntityViewModel<WorkingCirriculumViewModel> data = new EntityViewModel<WorkingCirriculumViewModel>
                    {
                        Result = Result.Ok
                    };

                    data.Entity = context.WorkingCirriculums.Where(o => o.Id == id).AsNoTracking().Select(o => new WorkingCirriculumViewModel
                    {
                        Id = o.Id,
                        AreaOfTrainingId = o.AreaOfTrainingId,
                        DepartmentId = o.DepartmentId,
                        EducationalStandart = o.EducationalStandart,
                        StartTraining = o.StartTraining,
                        TrainingPeriod = o.TrainingPeriod,
                        TypeOfEducationName = o.TypeOfEducationName,
                        DepartmentName = context.Departments.Where(d => !d.IsFaculty && d.Id == o.DepartmentId).Select(d => d.Name).FirstOrDefault(),
                        AreaOfTrainingName = context.AreasOfTraining.Where(a => a.Id == o.AreaOfTrainingId).Select(a => a.Name).FirstOrDefault()
                    }).FirstOrDefault();

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
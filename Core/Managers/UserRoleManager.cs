using Andromeda.Core.Logs;
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
    public class UserRoleManager
    {
        public static IViewModel GetUserAvailableRoles(string search, Guid? userId)
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    EntitiesViewModel<RoleViewModel> data = new EntitiesViewModel<RoleViewModel>
                    {
                        Result = Result.Ok
                    };

                    List<Guid> userRoleIds = new List<Guid>();
                    if (userId.HasValue)
                    {
                        userRoleIds = context.UserRoles.Where(o => o.UserId == userId).Select(o => o.RoleId).ToList();
                    }

                    data.Entities = context.Roles.Where(o => !userRoleIds.Contains(o.Id))
                        .Where(o => o.Name.ToLower().Contains((search ?? string.Empty).ToLower()))
                        .Select(o => new RoleViewModel
                        {
                            Id = o.Id,
                            Name = o.Name,
                            TiedToDepartment = context.RoleDepartments.Where(rd => rd.RoleId == o.Id).Count() > 0
                        })
                        .ToList();

                    return data;
                }
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel GetRoleAvailableDepartments(string search, Guid? userId, Guid? roleId)
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    EntitiesViewModel<DepartmentViewModel> data = new EntitiesViewModel<DepartmentViewModel>
                    {
                        Result = Result.Ok
                    };
                    var tempRoleDepartments = context.RoleDepartments.Where(o => o.RoleId == roleId).AsNoTracking();
                    var tempDepartmemts = context.Departments.Join(tempRoleDepartments,
                        d => d.Id,
                        rd => rd.DepartmentId,
                        (d, rd) => new DepartmentViewModel
                        {
                            Id = d.Id,
                            Name = d.Name,
                            ShortName = d.ShortName
                        })
                        .Where(o =>
                        o.Name.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                        o.ShortName.ToLower().Contains((search ?? string.Empty).ToLower())
                        ).ToList();

                    data.Entities = tempDepartmemts;

                    return data;
                }
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
    }
}

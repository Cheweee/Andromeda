using Andromeda.Core.Logs;
using Andromeda.Models.Administration;
using Andromeda.Models.Context;
using Andromeda.Models.RelationEntities;
using Andromeda.ViewModels.Server;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Core.Managers
{
    public class RoleManager : BaseEntityManager
    {
        public static Guid GetHeadOfDepartmentId()
        {
            return new Guid("6EA303EA-96CF-49E3-BB2E-56EB0ADC032C");
        }
        public static Guid GetDeanId()
        {
            return new Guid("AE9F0C57-DBB3-455F-A578-91C4CDCA3D79");
        }
        public static Guid GetSystemAdminId()
        {
            return new Guid("556CAB08-1CC0-40E7-B665-4E59E59189E4");
        }

        public static IViewModel GetNotRoleRights(Guid roleId, string search)
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    EntitiesViewModel<RightViewModel> data = new EntitiesViewModel<RightViewModel>
                    {
                        Result = Result.Ok
                    };

                    var tempEntities = context.RightRoles.Where(o => o.RoleId != roleId).AsNoTracking();

                    data.Entities = context.Rights.Join(tempEntities, r => r.Id, rr => rr.RightId, (r, rr) => new RightViewModel
                    {
                        Id = r.Id,
                        Name = r.Name
                    })
                    .Where(o=> o.Name.ToLower().Contains((search ?? string.Empty).ToLower()))
                    .ToList();

                    return data;
                }
            }
            catch(Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel SaveRoleRights(Guid roleId, List<Guid> ids)
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

                    var roleRights = context.RightRoles.Where(o => o.RoleId == roleId).AsNoTracking();
                    foreach(var role in roleRights.Where(o=> !ids.Contains(o.RightId)))
                    {
                        context.Entry(role).State = EntityState.Deleted;
                    }
                    foreach(var id in ids.Where(o=> !roleRights.Select(r => r.RightId).Contains(o)))
                    {
                        RightRole rightRole = new RightRole { RightId = id, RoleId = roleId };
                        context.Entry(rightRole).State = EntityState.Added;
                    }
                    context.SaveChanges();

                    context.Configuration.AutoDetectChangesEnabled = true;
                    context.Configuration.ValidateOnSaveEnabled = true;

                    return result;
                }
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
    }
}

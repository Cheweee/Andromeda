using Andromeda.Core.Logs;
using Andromeda.Models.Administration;
using Andromeda.Models.Context;
using Andromeda.ViewModels.Other;
using Andromeda.ViewModels.Server;
using Andromeda.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using WECr;
using System.Data.Entity;
using Andromeda.Models.RelationEntities;

namespace Andromeda.Core.Managers
{
    public class UserManager : BaseEntityManager
    {
        public static IViewModel Login(string login, string password, bool remember)
        {
            try
            {
                using (DBContext context = new DBContext())
                {
                    byte[] passwordHash = Encryption.Encrypt(password);
                    byte[] userPasswordHash = GetEntityValue<User, byte[]>(context, o => o.Login == login, o => o.Password);

                    if (userPasswordHash.SequenceEqual(passwordHash))
                    {
                        FormsAuthentication.SetAuthCookie(login, remember);
                        return new ResultViewModel { Result = Result.Ok };
                    }
                    return new ResultViewModel { Message = "Неверное имя пользователя или пароль", Result = Result.Error };
                }
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel LoadUserAccessiblePages()
        {            
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    PagesViewModel result = new PagesViewModel { Result = Result.Ok };
                    string login = HttpContext.Current.User.Identity.Name;
                    Guid userId = GetEntityValue<User, Guid>(context, o=> o.Login == login, o=> o.Id);

                    if (userId == Guid.Empty)
                    {
                        result.Result = Result.Error;
                        result.Message = "Ошибка входа пользователя!";

                        return result;
                    }
                    List<Guid> roleIds =
                        GetEntitiesWithJoin<User, UserRole, Guid, Guid>(context,
                        o => o.Login == login,
                        o => o.Id,
                        o => o.UserId,
                        (fo, so) => fo.RoleId
                        );
                    List<Guid> rightIds = GetEntitiesWithJoin<Role, RightRole, Guid, Guid>(context, o=> roleIds.Contains(o.Id), fo=>fo.Id, so=> so.RoleId, (fo, so) => fo.RightId);
                    result.AccesibleReferences = GetUserAccesibleReferences(context, rightIds);
                    result.AccesiblePages = GetUserAccesiblePages(context, rightIds);
                    result.AccesibleAdministration = GetUserAccesibleAdministration(context, rightIds);

                    return result;
                }
            }
            catch (Exception exc)
            {                
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel CanUserEditReferences()
        {
            try
            {
                using(DBContext context = DBContext.Create())
                {
                    ResultViewModel result = new ResultViewModel { Result = Result.Ok };
                    string login = HttpContext.Current.User.Identity.Name;
                    Guid userId = GetEntityValue<User, Guid>(context, o => o.Login == login, o => o.Id);
                    if (userId == Guid.Empty)
                    {
                        result.Result = Result.Error;
                        result.Message = "Ошибка входа пользователя!";

                        return result;
                    }

                    List<Guid> roleIds =
                        GetEntitiesWithJoin<User, UserRole, Guid, Guid>(context,
                        o => o.Login == login,
                        o => o.Id,
                        o => o.UserId,
                        (fo, so) => fo.RoleId
                        );
                    List<Guid> rightIds = GetEntitiesWithJoin<Role, RightRole, Guid, Guid>(context, o => roleIds.Contains(o.Id), fo => fo.Id, so => so.RoleId, (fo, so) => fo.RightId);

                    if(!rightIds.Contains(RightManager.GetAdminRightId()) || !rightIds.Contains(RightManager.GetEditReferencesId()))
                    {
                        result.Result = Result.NotEnoughRights;
                    }

                    return result;
                }
            }
            catch(Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel IsUserHeadOfDepartment()
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    ResultViewModel result = new ResultViewModel { Result = Result.Ok };
                    string login = HttpContext.Current.User.Identity.Name;
                    Guid userId = GetEntityValue<User, Guid>(context, o => o.Login == login, o => o.Id);
                    if (userId == Guid.Empty)
                    {
                        result.Result = Result.Error;
                        result.Message = "Ошибка входа пользователя!";

                        return result;
                    }

                    List<Guid> roleIds =
                        GetEntitiesWithJoin<User, UserRole, Guid, Guid>(context,
                        o => o.Login == login,
                        o => o.Id,
                        o => o.UserId,
                        (fo, so) => fo.RoleId
                        );

                    if (roleIds.Contains(RoleManager.GetHeadOfDepartmentId()))
                    {
                        result.Result = Result.Ok;
                    }
                    else
                    {
                        result.Result = Result.NotEnoughRights;
                    }

                    return result;
                }
            }
            catch(Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel IsUserDean()
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    ResultViewModel result = new ResultViewModel { Result = Result.Ok };
                    string login = HttpContext.Current.User.Identity.Name;
                    Guid userId = GetEntityValue<User, Guid>(context, o => o.Login == login, o => o.Id);
                    if (userId == Guid.Empty)
                    {
                        result.Result = Result.Error;
                        result.Message = "Ошибка входа пользователя!";

                        return result;
                    }

                    List<Guid> roleIds =
                        GetEntitiesWithJoin<User, UserRole, Guid, Guid>(context,
                        o => o.Login == login,
                        o => o.Id,
                        o => o.UserId,
                        (fo, so) => fo.RoleId
                        );

                    if (roleIds.Contains(RoleManager.GetDeanId()))
                    {
                        result.Result = Result.Ok;
                    }
                    else
                    {
                        result.Result = Result.NotEnoughRights;
                    }

                    return result;
                }
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel IsUserSystemAdmin()
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    ResultViewModel result = new ResultViewModel { Result = Result.Ok };
                    string login = HttpContext.Current.User.Identity.Name;
                    Guid userId = GetEntityValue<User, Guid>(context, o => o.Login == login, o => o.Id);
                    if (userId == Guid.Empty)
                    {
                        result.Result = Result.Error;
                        result.Message = "Ошибка входа пользователя!";

                        return result;
                    }

                    List<Guid> roleIds =
                        GetEntitiesWithJoin<User, UserRole, Guid, Guid>(context,
                        o => o.Login == login,
                        o => o.Id,
                        o => o.UserId,
                        (fo, so) => fo.RoleId
                        );

                    if (roleIds.Contains(RoleManager.GetSystemAdminId()))
                    {
                        result.Result = Result.Ok;
                    }
                    else
                    {
                        result.Result = Result.NotEnoughRights;
                    }

                    return result;
                }
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel GetUsers(int page, int limit, string order, bool isAscending, string search)
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    EntitiesViewModel<UserViewModel> data = new EntitiesViewModel<UserViewModel>
                    {
                        Result = Result.Ok
                    };
                    search = search ?? string.Empty;
                    string[] searchWords = search.ToLower().Contains(' ') ? search.Split(' ') : new string[] { search };

                    IQueryable<User> tempEntities = context.Users
                        .AsNoTracking();
                    if (searchWords.Length > 0)
                    {
                        foreach (string searchWord in searchWords)
                        {
                            tempEntities = tempEntities.Where(
                            o => o.UserName.ToLower().Contains(searchWord) ||
                            o.LastName.ToLower().Contains(searchWord) ||
                            o.Login.ToLower().Contains(searchWord) ||
                            o.Patronimyc.ToLower().Contains(searchWord));
                        }
                    }
                    List<UserViewModel> entities = tempEntities.Select(
                    o => new UserViewModel
                    {
                        Id = o.Id,
                        LastName = o.LastName,
                        Login = o.Login,
                        Name = o.UserName,
                        Patronimyc = o.Patronimyc ?? string.Empty
                    }).ToList();

                    data.Total = entities.Count;
                    data.Entities = entities
                        .OrderBy(order)
                        .Skip((page - 1) * limit)
                        .Take(limit)
                        .ToList();

                    return data;
                }
            }
            catch(Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel GetUser(Guid id)
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    EntityViewModel<UserViewModel> data = new EntityViewModel<UserViewModel>
                    {
                        Result = Result.Ok
                    };

                    User user = context.Users.Find(id);
                    
                    data.Entity = new UserViewModel
                    {
                        Id = user.Id,
                        LastName = user.LastName,
                        Login = user.Login,
                        Name = user.UserName,
                        Patronimyc = user.Patronimyc,
                        Password = Encryption.Decrypt(user.Password),
                        AcademicTitleId = user.AcademicTitleId
                    };

                    return data;
                }
            }
            catch(Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel GetNotUserAcademicDegrees(Guid userId, string search)
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    EntitiesViewModel<AcademicDegreeViewModel> data = new EntitiesViewModel<AcademicDegreeViewModel>
                    {
                        Result = Result.Ok
                    };

                    data.Entities = context.AcademicDegrees.Join(context.BranchesOfScience.AsNoTracking(),
                        a=> a.BranchOfScienceId,
                        b=>b.Id,
                        (a, b) => new AcademicDegreeViewModel
                        {
                            Id = a.Id,
                            Name = a.Name + " " + b.Name.ToLower(),
                            ShortName = a.ShortName + " " + b.ShortName.ToLower()
                        })
                        .AsNoTracking().ToList();

                    var userAdIds = context.AcademicDegreeUsers.Where(o => o.UserId == userId).Select(o => o.AcademicDegreeId).ToList();

                    if(userAdIds.Count > 0)
                    {
                        data.Entities = data.Entities.Where(o => !userAdIds.Contains(o.Id)).ToList();
                    }
                    data.Entities = data.Entities.Where(o =>
                         o.Name.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                         o.ShortName.ToLower().Contains((search ?? string.Empty).ToLower()))
                         .ToList();

                    return data;
                }
            }
            catch(Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel GetUserAcademicDegrees(Guid userId)
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    EntitiesViewModel<AcademicDegreeViewModel> data = new EntitiesViewModel<AcademicDegreeViewModel>
                    {
                        Result = Result.Ok
                    };

                    var tempEntities = context.AcademicDegreeUsers.Where(o => o.UserId == userId).AsNoTracking();

                    data.Entities = context.AcademicDegrees.Join(tempEntities, a => a.Id,
                         ua => ua.AcademicDegreeId,
                         (a, ua) => new AcademicDegreeViewModel
                         {
                             Id = a.Id,
                             Name = a.Name + " " + context.BranchesOfScience
                             .Where(b => b.Id == a.BranchOfScienceId)
                             .Select(b => b.Name).FirstOrDefault(),
                             ShortName = a.ShortName + " " + context.BranchesOfScience
                             .Where(b => b.Id == a.BranchOfScienceId).Select(b => b.ShortName)
                             .FirstOrDefault()
                         })
                         .AsNoTracking().ToList();

                    return data;
                }
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel GetUserRoles(Guid? userId)
        {
            try
            {
                using(DBContext context = DBContext.Create())
                {
                    EntitiesViewModel<UserRoleViewModel> data = new EntitiesViewModel<UserRoleViewModel>
                    {
                        Result = Result.Ok
                    };
                    
                    var tempUserRoles = context.UserRoles.Where(o => o.UserId == userId).AsNoTracking();
                    var tempRoles = context.Roles.Join(tempUserRoles,
                        r => r.Id,
                        ur => ur.RoleId,
                        (r, ur) => new UserRoleViewModel
                        {
                            Id = ur.Id,
                            RoleId = ur.RoleId,
                            DepartmentId = ur.DepartmentId,
                            UserId = ur.UserId,
                            Name = r.Name + (ur.DepartmentId.HasValue ? " " + context.Departments.Where(o => o.Id == ur.DepartmentId).Select(o => o.ShortName).FirstOrDefault() : string.Empty),
                            EntityState = EntityState.Unchanged
                        }).ToList();
                    data.Total = tempRoles.Count;
                    data.Page = 1;

                    data.Entities = tempRoles;

                    return data;
                }
            }
            catch(Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel SaveUser(UserViewModel viewModel)
        {
            try
            {
                using(DBContext context = DBContext.Create())
                {
                    AddOrEditViewModel data = new AddOrEditViewModel
                    {
                        Result = Result.Ok
                    };
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Configuration.ValidateOnSaveEnabled = false;

                    if (viewModel.Id == Guid.Empty)
                    {
                        User user = new User
                        {
                            Id = Guid.NewGuid(),
                            AcademicTitleId = viewModel.AcademicTitleId,
                            LastName = viewModel.LastName,
                            Login = viewModel.Login,
                            Password = Encryption.Encrypt(viewModel.Password),
                            Patronimyc = viewModel.Patronimyc,
                            UserName = viewModel.Name
                        };
                        context.Entry(user).State = EntityState.Added;
                        data.Id = user.Id;
                    }
                    else
                    {
                        User user = new User
                        {
                            Id = viewModel.Id,
                            AcademicTitleId = viewModel.AcademicTitleId,
                            LastName = viewModel.LastName,
                            Login = viewModel.Login,
                            Password = Encryption.Encrypt(viewModel.Password),
                            Patronimyc = viewModel.Patronimyc,
                            UserName = viewModel.Name
                        };
                        context.Entry(user).State = EntityState.Modified;
                        data.Id = user.Id;
                    }

                    context.SaveChanges();
                    context.Configuration.AutoDetectChangesEnabled = true;
                    context.Configuration.ValidateOnSaveEnabled = true;

                    return data;
                }
            }
            catch(Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel SaveUserRolesSate(List<UserRoleViewModel> viewModels)
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    ResultViewModel data = new ResultViewModel
                    {
                        Result = Result.Ok
                    };
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Configuration.ValidateOnSaveEnabled = false;

                    foreach (UserRoleViewModel viewModel in viewModels)
                    {
                        UserRole userRole = new UserRole
                        {
                            Id = viewModel.Id == Guid.Empty ? Guid.NewGuid() : viewModel.Id,
                            DepartmentId = viewModel.DepartmentId,
                            RoleId = viewModel.RoleId,
                            UserId = viewModel.UserId
                        };
                        context.Entry(userRole).State = viewModel.EntityState;
                    }
                    context.SaveChanges();

                    context.Configuration.AutoDetectChangesEnabled = true;
                    context.Configuration.ValidateOnSaveEnabled = true;

                    return data;
                }
            }
            catch(Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel SaveUserAcademicDegrees(Guid userId, List<Guid> ids)
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

                    var entities = context.AcademicDegreeUsers.Where(o => o.UserId == userId).AsNoTracking();
                    foreach (var entity in entities.Where(o => !ids.Contains(o.UserId)))
                    {
                        context.Entry(entity).State = EntityState.Deleted;
                    }
                    foreach (var id in ids.Where(o => !entities.Select(r => r.AcademicDegreeId).Contains(o)))
                    {
                        AcademicDegreeUser rightRole = new AcademicDegreeUser { AcademicDegreeId = id, UserId = userId };
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

        private static List<Page> GetUserAccesibleReferences(DBContext context, List<Guid> rightIds)
        {
            if (rightIds.Contains(RightManager.GetAccessToReferencesId()) ||
                rightIds.Contains(RightManager.GetAdminRightId()))
            {
                return new List<Page>
                {
                    new Page("/References/TypesOfProjects", "typesOfProjects", "Типы работ", "format_list_numbered"),
                    new Page("/References/CourseTitles", "coureTitles", "Наименования дисциплин", "title"),
                    new Page("/References/Faculties", "faculties", "Факультеты и институты", "business"),
                    new Page("/References/Departments", "departments", "Кафедры", "account_balance"),
                };
            }
            else
            {
                return new List<Page>();
            }
        }
        private static List<Page> GetUserAccesiblePages(DBContext context, List<Guid> rightIds)
        {
            List<Page> accesiblePages = new List<Page>();
            if (rightIds.Contains(new Guid("BDD85C56-7790-42E0-B8AA-2304E81761F9")))
            {                
                return new List<Page>
                {
                    new Page("/CirriculumDevelopment/Seats", "seats", "Должности", "event_seat"),
                    new Page("/CirriculumDevelopment/Workers", "workers", "Работники", "assignment_ind"),
                    new Page("/CirriculumDevelopment/AreasOfTraining", "areasOfTraining", "Направления подготовки", "layers"),
                    new Page("/CirriculumDevelopment/WorkingCirriculums", "workingCirriculums", "Рабочие планы", "chrome_reader_mode")
                };                
            }
            if (rightIds.Contains(RightManager.GetAccessToDepartmentRolesId()) || 
                rightIds.Contains(RightManager.GetAccessToFacultyRolesId()))
            {
                accesiblePages.Add(new Page("/CirriculumDevelopment/Seats", "seats", "Должности", "event_seat"));
            }
            if (rightIds.Contains(RightManager.GetAccessToDepartmentWorkersId()) ||
                rightIds.Contains(RightManager.GetAccessToFacultyWorkersId()))
            {
                accesiblePages.Add(new Page("/CirriculumDevelopment/Professors", "professors", "Работники", "assignment_ind"));
            }
            if (rightIds.Contains(RightManager.GetAccessToAreasOfTrainingId()))
            {
                accesiblePages.Add(new Page("/CirriculumDevelopment/AreasOfTraining", "areasOfTraining", "Направления подготовки", "layers"));
            }
            if (rightIds.Contains(RightManager.GetAccessToWorkingCirriculumsId()))
            {
                accesiblePages.Add(new Page("/CirriculumDevelopment/WorkingCirriculums", "workingCirriculums", "Рабочие планы", "chrome_reader_mode"));
            }
            return accesiblePages;
        }
        private static List<Page> GetUserAccesibleAdministration(DBContext context, List<Guid> rightIds)
        {
            if (rightIds.Contains(RightManager.GetAdminRightId()))
            {
                return new List<Page>
                {
                    new Page("/Administration/Users", "users", "Пользователи", "people"),
                    new Page("/Administration/Roles", "roles", "Роли", "work"),
                    new Page("/Administration/Rights", "rights", "Права ролей", "accessibility")
                };
            }
            List<Page> accesibleAdministration = new List<Page>();
            if (rightIds.Contains(RightManager.GetUsersAdminRightId()))
            {
                accesibleAdministration.Add(new Page("/Administration/Users", "users", "Пользователи", "people"));
            }
            if (rightIds.Contains(RightManager.GetRolesAdminRightId()))
            {
                accesibleAdministration.Add(new Page("/Administration/Roles", "roles", "Роли", "work"));
            }
            if (rightIds.Contains(RightManager.GetRightsAdminRightId()))
            {
                accesibleAdministration.Add(new Page("/Administration/Rights", "rights", "Права ролей", "accessibility"));
            }
            return accesibleAdministration;
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
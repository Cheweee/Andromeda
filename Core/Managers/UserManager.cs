using Andromeda.Core.Logs;
using Andromeda.Models.Administration;
using Andromeda.Models.Context;
using Andromeda.ViewModels.Other;
using Andromeda.ViewModels.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using WECr;

namespace Andromeda.Core.Managers
{
    public class UserManager : BaseEntityManager
    {
        public static ResultViewModel Login(string login, string password, bool remember)
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
                        GetEntitiesWithJoin<User, UserRoles, Guid, Guid>(context,
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
                        GetEntitiesWithJoin<User, UserRoles, Guid, Guid>(context,
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
                        GetEntitiesWithJoin<User, UserRoles, Guid, Guid>(context,
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
                        GetEntitiesWithJoin<User, UserRoles, Guid, Guid>(context,
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

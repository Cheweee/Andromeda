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
                    byte[] userPasswordHash = GetItemValue<User, byte[]>(context, o => o.Login == login, o => o.Password);

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
                    PagesViewModel result = new PagesViewModel();
                    string login = HttpContext.Current.User.Identity.Name;
                    Guid userId = GetItemValue<User, Guid>(context, o=> o.Login == login, o=> o.Id);
                    List<Guid> roleIds =
                        GetCollectionWithJoin<User, UserRoles, Guid, Guid>(context,
                        o => o.Login == login,
                        o => o.Id,
                        o => o.UserId,
                        (fo, so) => fo.RoleId
                        );

                    if (userId == Guid.Empty)
                    {
                        result.Result = Result.Error;
                        result.Message = "Ошибка входа пользователя!";

                        return result;
                    }
                    result.AccesibleReferences = GetUserAccesibleReferences(context, roleIds);
                    result.AccesiblePages = GetUserAccesiblePages(context, roleIds);
                    result.AccesibleAdministration = GetUserAccesibleAdministration(context, roleIds);
                    result.Result = Result.Ok;

                    return result;
                }
            }
            catch (Exception exc)
            {                
                return LogErrorManager.Add(exc);
            }
        }

        private static List<Page> GetUserAccesibleReferences(DBContext context, List<Guid> roleIds)
        {
            if (roleIds.Contains(new Guid("70D3CB1F-48BA-44C5-B551-3A19DE0F32C1")) ||
                roleIds.Contains(new Guid("BDD85C56-7790-42E0-B8AA-2304E81761F9")))
            {
                return new List<Page>
                {
                    new Page("/CirriculumDevelopment/TypesOfProjects", "types_of_projects", "Типы работ", "format_list_numbered"),
                    new Page("/CirriculumDevelopment/TypesOfProjects", "coure_titles", "Наименования дисциплин", "title"),
                    new Page("/CirriculumDevelopment/Faculties", "faculties", "Факультеты и институты", "business"),
                    new Page("/CirriculumDevelopment/Departments", "departments", "Кафедры", "account_balance"),
                };
            }
            else
            {
                return new List<Page>();
            }
        }
        private static List<Page> GetUserAccesiblePages(DBContext context, List<Guid> roleIds)
        {
            List<Page> accessiblePages = new List<Page>();
            if (roleIds.Contains(new Guid("BDD85C56-7790-42E0-B8AA-2304E81761F9")))
            {                
                accessiblePages.AddRange(new List<Page>
                {
                    new Page("", "seats", "Должности", "event_seat"),
                    new Page("", "professors", "Преподаватели", "assignment_ind"),
                    new Page("", "areas_of_training", "Направления подготовки", "layers"),
                    new Page("", "working_cirriculum", "Рабочие планы", "chrome_reader_mode")
                }.ToList());
            }
            if (roleIds.Contains(new Guid("E0AFAFE5-D9C8-40BE-AF1D-A2444DA1D38B")))
            {

            }
            return accessiblePages;
        }
        private static List<Page> GetUserAccesibleAdministration(DBContext context, List<Guid> roleIds)
        {
            List<Page> accesibleAdministration = new List<Page>
                {
                    new Page("/Administration/Users", "users", "Пользователи", "people"),
                    new Page("/Administration/Roles", "roles", "Роли", "work"),
                    new Page("/Administration/Rights", "rights", "Права ролей", "accessibility")
                };
            return accesibleAdministration;
        }
    }
}

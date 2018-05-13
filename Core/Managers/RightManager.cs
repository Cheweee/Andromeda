using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Core.Managers
{
    public class RightManager
    {
        public static Guid GetAccessToReferencesId()
        {
            return new Guid("70D3CB1F-48BA-44C5-B551-3A19DE0F32C1");
        }
        public static Guid GetAccessToFacultyWorkersId()
        {
            return new Guid("E0AFAFE5-D9C8-40BE-AF1D-A2444DA1D38B");
        }
        public static Guid GetAccessToDepartmentWorkersId()
        {
            return new Guid("61683D42-2511-43A4-9C54-389BF6DD7F3A");
        }
        public static Guid GetAccessToFacultyRolesId()
        {
            return new Guid("76C77E38-21FF-441D-8053-8F8915084B47");
        }
        public static Guid GetAccessToDepartmentRolesId()
        {
            return new Guid("B63143D4-88E5-44C9-A405-A06FAAA0CC06");
        }
        public static Guid GetAccessToAreasOfTrainingId()
        {
            return new Guid("8753EF72-BE1B-48C1-996D-DFEF39C96DE5");
        }
        public static Guid GetAccessToWorkingCirriculumsId()
        {
            return new Guid("0EF53537-45CD-4301-9DC0-F7082593C9F9");
        }
        public static Guid GetAdminRightId()
        {
            return new Guid("BDD85C56-7790-42E0-B8AA-2304E81761F9");
        }
        public static Guid GetUsersAdminRightId()
        {
            return new Guid("022594F5-A3CF-43E9-BC08-6678EC4A930D");
        }
        public static Guid GetRolesAdminRightId()
        {
            return new Guid("5BE3F4FC-5C04-4C4C-BBB0-C4CB890408FB");
        }
        public static Guid GetRightsAdminRightId()
        {
            return new Guid("859F655B-4F8D-417C-AB54-CBE529373B81");
        }
    }
}

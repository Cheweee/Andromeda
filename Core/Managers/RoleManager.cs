using System;
using System.Collections.Generic;
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
    }
}

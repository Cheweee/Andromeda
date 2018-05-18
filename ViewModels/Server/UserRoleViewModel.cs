using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.ViewModels.Server
{
    public class UserRoleViewModel : IKeyViewModel<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public Guid? DepartmentId { get; set; }
        public string Name { get; set; }
        public EntityState EntityState { get; set; }
    }
}

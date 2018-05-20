using Andromeda.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.RelationEntities
{
    public class RoleDepartment : IKeyEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public Guid DepartmentId { get; set; }
    }
}

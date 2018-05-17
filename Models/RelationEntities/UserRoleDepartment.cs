using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.RelationEntities
{
    public class UserRoleDepartment
    {
        [Key]
        [Column(Order = 1)]
        public Guid UserId { get; set; }
        [Key]
        [Column(Order = 2)]
        public Guid RoleId { get; set; }
        [Key]
        [Column(Order =3)]
        public Guid DepartmentId { get; set; }
    }
}

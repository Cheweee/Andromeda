using Andromeda.Models.Administration;
using Andromeda.Models.Interfaces;
using Andromeda.Models.References;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.RelationEntities
{
    /// <summary>
    /// The entity for many-to-many communication user - role
    /// </summary>
    public partial class UserRole : IKeyEntity<Guid>
    {
        #region Properties
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public Guid? DepartmentId { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
        public virtual Department Department { get; set; }
        #endregion
    }
}

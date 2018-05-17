using Andromeda.Models.Administration;
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
    public partial class UserRole
    {
        #region Properties
        [Key]
        [Column(Order = 1)]
        public Guid UserId { get; set; }
        [Key]
        [Column(Order = 2)]
        public Guid RoleId { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<UserRoleDepartment> UserRoleDepartments { get; set; }
        #endregion

        #region Constructors
        public UserRole()
        {
            this.UserRoleDepartments = new HashSet<UserRoleDepartment>();
        }
        #endregion
    }
}

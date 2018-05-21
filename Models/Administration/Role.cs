using Andromeda.Models.Interfaces;
using Andromeda.Models.References;
using Andromeda.Models.RelationEntities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.Administration
{
    /// <summary>
    /// User role entity
    /// </summary>
    public partial class Role : IKeyEntity<Guid>, INameEntity, IRole<Guid>
    {
        #region Properties
        /// <summary>
        /// Id of the role (required property)
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Name of the role (required property)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Can user teach students (show that user take place in load distribution)
        /// </summary>
        public bool CanTeach { get; set; }
        /// <summary>
        /// Collection of users which have this role
        /// </summary>
        public virtual ICollection<UserRole> Users { get; set; }
        /// <summary>
        /// Collection of rights of the role
        /// </summary>
        public virtual ICollection<RightRole> RightRoles { get; set; }
        public virtual ICollection<RoleDepartment> RoleDepartments { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public Role()
        {
            this.Users = new HashSet<UserRole>();
            this.RightRoles = new HashSet<RightRole>();
            this.RoleDepartments = new HashSet<RoleDepartment>();
        }
        #endregion
    }
}

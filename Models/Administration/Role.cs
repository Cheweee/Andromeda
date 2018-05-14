﻿using Andromeda.Models.Interfaces;
using Andromeda.Models.References;
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
    public partial class Role : IKeyEntity<Guid>, INameEntity ,IRole<Guid>
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
        public virtual ICollection<UserRoles> UserRoles { get; set; }
        /// <summary>
        /// Collection of rights of the role
        /// </summary>
        public virtual ICollection<RightRole> RightRoles { get; set; }
        /// <summary>
        /// Collection of departments which include the role
        /// </summary>
        public virtual ICollection<Department> Departments { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public Role()
        {
            this.UserRoles = new HashSet<UserRoles>();
            this.RightRoles = new HashSet<RightRole>();
            this.Departments = new HashSet<Department>();
        }
        #endregion
    }
}

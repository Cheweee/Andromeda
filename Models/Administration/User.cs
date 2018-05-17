using Andromeda.Models.Interfaces;
using Andromeda.Models.References;
using Andromeda.Models.RelationEntities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.Administration
{
    /// <summary>
    /// User of the system entity
    /// </summary>
    public partial class User : IKeyEntity<Guid>, IUser<Guid>
    {
        #region Properties
        /// <summary>
        /// Id of the user
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Academic title id
        /// </summary>
        public Guid? AcademicTitleId { get; set; }
        /// <summary>
        /// User name (required property)
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        /// <summary>
        /// User last name (required property)
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        /// <summary>
        /// User login (required property)
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Login { get; set; }
        /// <summary>
        /// User patronymic
        /// </summary>
        [MaxLength(50)]
        public string Patronimyc { get; set; }
        /// <summary>
        /// User password (required property)
        /// </summary>
        public byte[] Password { get; set; }
        /// <summary>
        /// Collection of user roles
        /// </summary>
        public virtual ICollection<UserRole> UserRoles { get; set; }
        /// <summary>
        /// Collection of user academic degrees
        /// </summary>
        public virtual ICollection<AcademicDegreeUser> AcademicDegreeUsers { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public User()
        {
            this.UserRoles = new HashSet<UserRole>();
            this.AcademicDegreeUsers = new HashSet<AcademicDegreeUser>();
        }
        #endregion
    }
}

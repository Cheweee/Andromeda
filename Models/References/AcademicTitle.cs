using Andromeda.Models.Administration;
using Andromeda.Models.Interfaces;
using Andromeda.Models.RelationEntities;
using System;
using System.Collections.Generic;

namespace Andromeda.Models.References
{
    /// <summary>
    /// Academic title of user (user cannot change this reference)
    /// </summary>
    public partial class AcademicTitle: IKeyEntity<Guid>, INameEntity, IShortNameEntity
    {
        #region Properties
        /// <summary>
        /// Id of the academic title
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Name of the academic title (required property)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Short name of the academic title (required property)
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Collection of users who have this academic title
        /// </summary>
        public virtual ICollection<User> Users { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public AcademicTitle()
        {
            this.Users = new HashSet<User>();
        }
        #endregion
    }
}

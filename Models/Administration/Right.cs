using Andromeda.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.Administration
{
    /// <summary>
    /// User right entity
    /// </summary>
    public partial class Right : IKeyEntity<Guid>, INameEntity
    {
        #region Properties
        /// <summary>
        /// Id of the right (required property)
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Name of the right (required property)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Roles collection
        /// </summary>
        public virtual ICollection<RightRole> RightRoles { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public Right()
        {
            this.RightRoles = new HashSet<RightRole>();
        }
        #endregion
    }
}

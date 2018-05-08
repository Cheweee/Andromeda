using Andromeda.Models.Administration;
using Andromeda.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.References
{
    /// <summary>
    /// Academic degree of user (user cannot change this reference)
    /// </summary>
    public partial class AcademicDegree : IKeyEntity<Guid>, INameEntity, IShortNameEntity
    {
        #region Properties
        /// <summary>
        /// Id of the academic degree
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Name of the academic degree (required property)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Short name of the academic degree (required property)
        /// </summary>
        public string ShortName { get; set; }
        /// <summary>
        /// Id of the brach of science
        /// </summary>
        public Guid BranchOfScienceId { get; set; }

        /// <summary>
        /// Collection of users who have this academic degree
        /// </summary>
        public virtual ICollection<User> Users { get; set; }
        /// <summary>
        /// Branch of science of this academic degree
        /// </summary>
        public virtual BranchOfScience BranchOfScience { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public AcademicDegree()
        {
            this.Users = new HashSet<User>();
        }
        #endregion
    }
}

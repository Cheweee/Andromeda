using Andromeda.Models.Entities;
using Andromeda.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.References
{
    /// <summary>
    /// Type of project entity
    /// </summary>
    public partial class TypeOfProject : IKeyEntity<Guid>, INameEntity, IShortNameEntity
    {
        #region Properties
        /// <summary>
        /// Id of the type of project
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Name of the type of project (required property)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Short name of the type of project (required property)
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Collection of projects which belongs to this type of project
        /// </summary>
        public virtual ICollection<Project> Projects { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public TypeOfProject()
        {
            this.Projects = new HashSet<Project>();
        }
        #endregion
    }
}

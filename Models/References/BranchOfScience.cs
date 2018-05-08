using Andromeda.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.References
{
    /// <summary>
    /// Branch of science (user cannot change this reference)
    /// </summary>
    public class BranchOfScience : IKeyEntity<Guid>, INameEntity, IShortNameEntity
    {
        #region Properties
        /// <summary>
        /// Id of the branch of science
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Name of the branch of science (required property)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Short name of the branch of science (required property)
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Collection of academic degrees which belongs to the branch of science
        /// </summary>
        public virtual ICollection<AcademicDegree> AcademicDegrees { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public BranchOfScience()
        {
            AcademicDegrees = new HashSet<AcademicDegree>();
        }
        #endregion
    }
}

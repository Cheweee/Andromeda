using Andromeda.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.References
{
    /// <summary>
    /// Type of education entity
    /// </summary>
    public partial class TypeOfEducation
    {
        #region Properties
        /// <summary>
        /// Name of the type of education (key of the entity)
        /// </summary>
        [Key]
        public string Name { get; set; }

        /// <summary>
        /// Collection of working cirriculums which belongs to this type of education
        /// </summary>
        public virtual ICollection<WorkingCirriculum> WorkingCirriculum { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public TypeOfEducation()
        {
            this.WorkingCirriculum = new HashSet<WorkingCirriculum>();
        }
        #endregion
    }
}

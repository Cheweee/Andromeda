using Andromeda.Models.Entities;
using Andromeda.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.References
{
    /// <summary>
    /// Level of higher education (user cannot edit this reference)
    /// </summary>
    public partial class LevelOfHigherEducation: INameEntity
    {
        #region Properties
        /// <summary>
        /// Name of the level of higher education
        /// </summary>
        [Key]
        public string Name { get; set; }

        /// <summary>
        /// Collection of areas of training which belongs to this level of higher education
        /// </summary>
        public virtual ICollection<AreaOfTraining> AreasOfTraining { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public LevelOfHigherEducation()
        {
            this.AreasOfTraining = new HashSet<AreaOfTraining>();
        }
        #endregion
    }
}

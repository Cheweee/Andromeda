using Andromeda.Models.Interfaces;
using Andromeda.Models.References;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.Entities
{
    /// <summary>
    /// Area of training entity
    /// </summary>
    public partial class AreaOfTraining : IKeyEntity<Guid>, INameEntity, IShortNameEntity
    {
        #region Properties
        /// <summary>
        /// Id of the area of training
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Name of the area of training (required property)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Short name of the area of training (required property)
        /// </summary>
        public string ShortName { get; set; }
        /// <summary>
        /// Code of the area of training (required property)
        /// </summary>
        [Required]
        public double Code { get; set; }
        /// <summary>
        /// Directionaly of the area of training (required property)
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Directionaly { get; set; }
        /// <summary>
        /// Level of higher education name which contains this area of training (required property)
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string LevelOfHigherEducationName { get; set; }

        /// <summary>
        /// Level of higher education entity
        /// </summary>
        public virtual LevelOfHigherEducation LevelOfHigherEducation { get; set; }
        /// <summary>
        /// Collection of working cirriculums which belongs to this area of training
        /// </summary>
        public virtual ICollection<WorkingCirriculum> WorkingCirriculums { get; set; }
        #endregion
        
        #region Constructors
        /// <summary>
        /// Default constructor`
        /// </summary>
        public AreaOfTraining()
        {
            this.WorkingCirriculums = new HashSet<WorkingCirriculum>();
        }
        #endregion
    }
}

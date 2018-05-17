using Andromeda.Models.Entities;
using Andromeda.Models.Interfaces;
using Andromeda.Models.RelationEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.References
{
    /// <summary>
    /// Comptence of the academic discipline entity
    /// </summary>
    public partial class Competence : IKeyEntity<Guid>
    {
        #region Properties
        /// <summary>
        /// Id of the comptenece
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Code of the competence (required property)
        /// </summary>
        [Required]
        public int Code { get; set; }
        /// <summary>
        /// Description of the competence (required property)
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Collection of academic disciplines which have this competence
        /// </summary>
        public virtual ICollection<CompetenceAcademicDiscipline> CompetenceAcademicDisciplines { get; set; }
        /// <summary>
        /// Collection of working cirriculums which have this competence
        /// </summary>
        public virtual ICollection<WorkingCirriculum> WorkingCirriculums { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public Competence()
        {
            this.CompetenceAcademicDisciplines = new HashSet<CompetenceAcademicDiscipline>();
            this.WorkingCirriculums = new HashSet<WorkingCirriculum>();
        }
        #endregion
    }
}

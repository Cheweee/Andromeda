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
    /// Project entity
    /// </summary>
    public class Project: IKeyEntity<Guid>
    {
        #region Properties
        /// <summary>
        /// Id of the project
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Id of the academic discipline which contains this project (required property)
        /// </summary>
        [Required]
        public Guid AcademicDisciplineId { get; set; }
        /// <summary>
        /// Id of the type of project which belongs to this project (required property)
        /// </summary>
        [Required]
        public Guid TypeOfProjectId { get; set; }
        /// <summary>
        /// Semester of the project (required property)
        /// </summary>
        [Required]
        public int Semester { get; set; }
        /// <summary>
        /// Capacity of the project (required property)
        /// </summary>
        [Required]
        public int Capacity { get; set; }

        /// <summary>
        /// Type of project entity
        /// </summary>
        public virtual TypeOfProject TypeOfProject { get; set; }
        /// <summary>
        /// Academic discipline entity
        /// </summary>
        public virtual AcademicDiscipline AcademicDiscipline { get; set; }
        #endregion
    }
}

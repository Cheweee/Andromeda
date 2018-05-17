using Andromeda.Models.Interfaces;
using Andromeda.Models.References;
using Andromeda.Models.RelationEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.Entities
{
    /// <summary>
    /// Academic discipline entity
    /// </summary>
    public partial class AcademicDiscipline: IKeyEntity<Guid>
    {
        #region Properties
        /// <summary>
        /// Id of the academic discipline
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Id of the course title of this academic discipline (required property)
        /// </summary>
        [Required]
        public Guid CourseTitleId { get; set; }
        /// <summary>
        /// Id of the working cirriculum which contains this academic discipline
        /// </summary>
        public Guid? WorkingCirriculumId { get; set; }
        /// <summary>
        /// Id of the department which contains this academic discipline
        /// </summary>
        public Guid? DepartmentId { get; set; }
        /// <summary>
        /// Index of academic discipline (required property)
        /// </summary>
        [Required]
        [MaxLength(15)]
        public string Index { get; set; }
        public int? SUTExpert { get; set; }
        public int? SUTFactual { get; set; }
        public int? SUTTotalOurs { get; set; }
        public int? TotalOursOnPlan { get; set; }
        public int? AllInInteractiveForm { get; set; }
        public int? AllInElectronicForm { get; set; }
        public int? Contact { get; set; }
        public int? IWOS { get; set; }
        public int? Control { get; set; }

        /// <summary>
        /// Course title entity of this academic discipline
        /// </summary>
        public virtual CourseTitle CourseTitle { get; set; }
        /// <summary>
        /// Working cirriculum entity which contains this academic discipline
        /// </summary>
        public virtual WorkingCirriculum WorkingCirriculum { get; set; }
        /// <summary>
        /// Department entity which contains this academic discipline
        /// </summary>
        public virtual Department Department { get; set; }
        /// <summary>
        /// Collection of competences which need to this academic discipline
        /// </summary>
        public virtual ICollection<CompetenceAcademicDiscipline> CompetenceAcademicDisciplines { get; set; }
        /// <summary>
        /// Collection of projects of this academic discipline
        /// </summary>
        public virtual ICollection<Project> Projects { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public AcademicDiscipline()
        {
            this.CompetenceAcademicDisciplines = new HashSet<CompetenceAcademicDiscipline>();
            this.Projects = new HashSet<Project>();
        }
        #endregion
    }
}

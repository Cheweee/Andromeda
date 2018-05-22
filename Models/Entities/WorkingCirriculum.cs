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
    /// Working cirriculum entity
    /// </summary>
    public partial class WorkingCirriculum: IKeyEntity<Guid>
    {
        #region Properties
        /// <summary>
        /// Id of the working cirriculum
        /// </summary>
        public Guid Id { get; set; }        
        /// <summary>
        /// Id of the department which contains this working cirriculum (required property)
        /// </summary>
        [Required]
        public Guid DepartmentId { get; set; }
        /// <summary>
        /// Area of training of this working cirriculum (required property)
        /// </summary>
        [Required]
        public Guid AreaOfTrainingId { get; set; }

        public Guid? StudyGroupId { get; set; }
        public Guid? WorkingCirriculumFileId { get; set; }
        /// <summary>
        /// Type of education name of this working cirriculum (required property)
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string TypeOfEducationName { get; set; }
        /// <summary>
        /// Training period of this working cirriculum (required property)
        /// </summary>
        [Required]
        public double TrainingPeriod { get; set; }
        /// <summary>
        /// Start training year of this working cirriculum (required property)
        /// </summary>
        [Required]
        public int StartTraining { get; set; }
        /// <summary>
        /// Educational standart of this working cirriculum (required property)
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string EducationalStandart { get; set; }

        /// <summary>
        /// Area of training entity
        /// </summary>
        public virtual AreaOfTraining AreaOfTraining { get; set; }
        /// <summary>
        /// Type of education entity
        /// </summary>
        public virtual TypeOfEducation TypeOfEducation { get; set; }
        /// <summary>
        /// Department entity
        /// </summary>
        public virtual Department Department { get; set; }
        public virtual WorkingCirriculumFile WorkingCirriculumFile { get; set; }
        public virtual StudyGroup StudyGroup { get; set; }
        /// <summary>
        /// Collection of competences that need to this working cirriculum
        /// </summary>
        public virtual ICollection<Competence> Competences { get; set; }
        /// <summary>
        /// Collection of academic disciplines of this working cirriculum
        /// </summary>
        public virtual ICollection<AcademicDiscipline> AcademicDisciplines { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public WorkingCirriculum()
        {
            this.Competences = new HashSet<Competence>();
            this.AcademicDisciplines = new HashSet<AcademicDiscipline>();
        }
        #endregion
    }
}

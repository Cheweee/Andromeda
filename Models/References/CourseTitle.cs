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
    /// Course title entity
    /// </summary>
    public partial class CourseTitle : IKeyEntity<Guid>, INameEntity, IShortNameEntity
    {
        #region Property
        /// <summary>
        /// Id of the course title
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Name of the course title (required property)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Short name og the course title (required property)
        /// </summary>
        public string ShortName { get; set; }
        /// <summary>
        /// Id of the department which contains this course title (required property)
        /// </summary>
        [Required]
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// Collection of the academic disciplines which contains this course title
        /// </summary>
        public virtual ICollection<AcademicDiscipline> AcademicDisciplines { get; set; }
        /// <summary>
        /// Department which contains this course title (required property)
        /// </summary>
        public virtual Department Department { get; set; }
        #endregion
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public CourseTitle()
        {
            this.AcademicDisciplines = new HashSet<AcademicDiscipline>();
        }
        #endregion
    }
}

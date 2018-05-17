using Andromeda.Models.Administration;
using Andromeda.Models.Entities;
using Andromeda.Models.Interfaces;
using Andromeda.Models.RelationEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.References
{
    /// <summary>
    /// Department entity
    /// </summary>
    public partial class Department : IKeyEntity<Guid>, INameEntity, IShortNameEntity
    {
        #region Properties
        /// <summary>
        /// Id of the department
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Name of the department (required property)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Short name of the department (required property)
        /// </summary>
        public string ShortName { get; set; }
        /// <summary>
        /// Code of the department
        /// </summary>
        public int? Code { get; set; }
        /// <summary>
        /// Is faculty or department
        /// </summary>
        public bool IsFaculty { get; set; }
        /// <summary>
        /// Faculty id
        /// </summary>
        public Guid? FacultyId { get; set; }

        /// <summary>
        /// Collection of user roles in this department
        /// </summary>
        public virtual ICollection<UserRoleDepartment> UserRoleDepartments { get; set; }
        /// <summary>
        /// Collection of working cirriculums in this department
        /// </summary>
        public virtual ICollection<WorkingCirriculum> WorkingCirriculums { get; set; }
        /// <summary>
        /// Collection of academic disciplines of this department
        /// </summary>
        public virtual ICollection<AcademicDiscipline> AcademicDisciplines { get; set; }
        /// <summary>
        /// Collection of course titles in this department
        /// </summary>
        public virtual ICollection<CourseTitle> CourseTitlesInDepartment { get; set; }
        /// <summary>
        /// Collection of areas of training in this department
        /// </summary>
        public virtual ICollection<AreaOfTraining> AreasOfTraining { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public Department()
        {
            this.IsFaculty = false;
            this.UserRoleDepartments = new HashSet<UserRoleDepartment>();
            this.WorkingCirriculums = new HashSet<WorkingCirriculum>();
            this.AcademicDisciplines = new HashSet<AcademicDiscipline>();
            this.CourseTitlesInDepartment = new HashSet<CourseTitle>();
            this.AreasOfTraining = new HashSet<AreaOfTraining>();
        }
        #endregion
    }
}

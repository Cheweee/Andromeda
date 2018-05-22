using Andromeda.Models.Interfaces;
using Andromeda.Models.References;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.Entities
{
    public class StudyGroup: IKeyEntity<Guid>
    {
        #region Properties
        public Guid Id { get; set; }

        public int YearOfTraining { get; set; }
        public string Name { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid WorkingCirriculumId { get; set; }

        public virtual Department Department { get; set; }
        public virtual WorkingCirriculum WorkingCirriculum { get; set; }
        #endregion
    }
}

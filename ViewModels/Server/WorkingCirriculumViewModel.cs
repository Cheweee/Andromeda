using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.ViewModels.Server
{
    public class WorkingCirriculumViewModel : IKeyViewModel<Guid>
    {
        public Guid Id { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid FacultyId { get; set; }
        public Guid AreaOfTrainingId { get; set; }
        public string TypeOfEducationName { get; set; }
        public string DepartmentName { get; set; }
        public string FacultyName { get; set; }
        public string AreaOfTrainingName { get; set; }
        public string EducationalStandart { get; set; }
        public double TrainingPeriod { get; set; }
        public int StartTraining { get; set; }
    }
}

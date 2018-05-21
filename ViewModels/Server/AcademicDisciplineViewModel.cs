using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.ViewModels.Server
{
    public class AcademicDisciplineViewModel : IKeyViewModel<Guid>
    {
        public Guid Id { get; set; }

        public string Code { get; set; }
        public string CourseTitle { get; set; }
        public Guid CourseTitleId { get; set; }

        public int TotalOursOnPlan { get; set; }
        public int SUTTotalOurs { get; set; }
        public int SUTExpert { get; set; }
        public int SUTFactual { get; set; }
        public int OursInInteractiveForm { get; set; }
        public int OursInElectronicalForm { get; set; }
        public int ContactOurs { get; set; }
        public int IWOSOurs { get; set; }
        public int ControlOurs { get; set; }

    }
}

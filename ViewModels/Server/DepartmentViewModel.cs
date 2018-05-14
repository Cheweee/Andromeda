using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.ViewModels.Server
{
    public class DepartmentViewModel : IKeyViewModel<Guid>, INameViewModel, IShortNameViewModel
    {
        public Guid Id { get; set; }
        public Guid? FacultyId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string FacultyName { get; set; }
        public string FacultyShortName { get; set; }
        public int Code { get; set; }
    }
}

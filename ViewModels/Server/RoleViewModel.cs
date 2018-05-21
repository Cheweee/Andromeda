using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.ViewModels.Server
{
    public class RoleViewModel : IKeyViewModel<Guid>, INameViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool CanTeach { get; set; }
        public bool TiedToDepartment { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.ViewModels.Server
{
    public class RightViewModel : IKeyViewModel<Guid>, INameViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}

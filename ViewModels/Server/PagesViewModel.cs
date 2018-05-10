using Andromeda.ViewModels.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.ViewModels.Server
{
    public class PagesViewModel : IViewModel
    {
        public Result Result { get; set; }
        public string Message { get; set; }
        public List<Page> AccesibleReferences { get; set; }
        public List<Page> AccesiblePages { get; set; }
        public List<Page> AccesibleAdministration { get; set; }
    }
}

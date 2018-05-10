using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.ViewModels.Server
{
    public class CollectionViewModel<T> : IViewModel
    {
        public Result Result { get; set; }
        public string Message { get; set; }
        public List<T> Collection { get; set; }
    }
}

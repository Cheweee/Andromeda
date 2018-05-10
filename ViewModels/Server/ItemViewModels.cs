using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.ViewModels.Server
{
    public class ItemViewModel<T> : IViewModel
    {
        public Result Result { get; set; }
        public string Message { get; set; }
        public T Item { get; set; }
    }
}

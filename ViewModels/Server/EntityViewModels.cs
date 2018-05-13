using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.ViewModels.Server
{
    public class EntityViewModel<TEntity> : IViewModel
    {
        public Result Result { get; set; }
        public string Message { get; set; }
        public TEntity Entity { get; set; }
    }
}

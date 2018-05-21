using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.ViewModels.Server
{
    public class EntitiesViewModel<TEntity> : IViewModel
    {
        public Result Result { get; set; }
        public string Message { get; set; }
        public List<TEntity> Entities { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
    }
}
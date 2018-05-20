using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.ViewModels.Client
{
    public class ChangeEntitiesViewModel<T>
    {
        public Guid NewId { get; set; }
        public List<T> Entities { get; set; } = new List<T>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.ViewModels.Client
{
    public class EntitiesViewModel
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public string Search { get; set; }
        public string Order { get; set; }
        public Guid SearchId { get; set; }

        public bool IsAscending()
        {
            if (Order.FirstOrDefault() != '-')
            {
                return true;
            }

            Order = Order.Remove(0, 1);
            return false;
        }
    }
}

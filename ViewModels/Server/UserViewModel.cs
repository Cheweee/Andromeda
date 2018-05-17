using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.ViewModels.Server
{
    public class UserViewModel : IKeyViewModel<Guid>
    {
        public Guid Id { get; set; }
        public Guid? AcademicTitleId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Patronimyc { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}

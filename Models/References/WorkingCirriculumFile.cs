using Andromeda.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.References
{
    public class WorkingCirriculumFile : IKeyEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid WorkingCirriculumId { get; set; }
        public byte[] FileData { get; set; }
    }
}

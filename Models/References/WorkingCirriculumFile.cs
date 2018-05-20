using Andromeda.Models.Entities;
using Andromeda.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.References
{
    public class WorkingCirriculumFile : IKeyEntity<Guid>
    {
        #region Properties
        public Guid Id { get; set; }
        public Guid WorkingCirriculumId { get; set; }
        public byte[] FileData { get; set; }
        public string FileExtension { get; set; }
        public double FileSize { get; set; }
        public string FileName { get; set; }
        public virtual WorkingCirriculum WorkingCirriculum { get; set; }
        #endregion
    }
}

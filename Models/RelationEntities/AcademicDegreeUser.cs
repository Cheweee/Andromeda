using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.RelationEntities
{
    public class AcademicDegreeUser
    {
        [Key]
        [Column(Order = 0)]
        public Guid AcademicDegreeId { get; set; }
        [Key]
        [Column(Order = 1)]
        public Guid UserId { get; set; }
    }
}

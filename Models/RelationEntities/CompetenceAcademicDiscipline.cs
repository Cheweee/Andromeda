using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.RelationEntities
{
    public class CompetenceAcademicDiscipline
    {
        [Key]
        [Column(Order = 1)]
        public Guid CompetenceId { get; set; }
        [Key]
        [Column(Order = 2)]
        public Guid AcademicDisciplineId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.RelationEntities
{
    public class RightRole
    {
        [Key]
        [Column(Order = 1)]
        public Guid RightId { get; set; }
        [Key]
        [Column(Order = 2)]
        public Guid RoleId { get; set; }
    }
}

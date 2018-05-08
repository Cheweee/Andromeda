using Andromeda.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.Logs
{
    public partial class LogError: IKeyEntity<Guid>
    {
        #region Properties
        public Guid Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Method { get; set; }
        [Required]
        [MaxLength(255)]
        public string Message { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        #endregion
    }
}

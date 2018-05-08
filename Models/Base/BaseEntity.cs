using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.Base
{
    /// <summary>
    /// Base entity class
    /// </summary>
    public class BaseEntity
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string ShortName { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        #endregion
    }
}

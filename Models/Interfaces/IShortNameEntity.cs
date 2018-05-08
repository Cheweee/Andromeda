using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.Interfaces
{
    /// <summary>
    /// Short name of the entity interface
    /// </summary>
    public interface IShortNameEntity
    {
        /// <summary>
        /// Short name of the entity (required property)
        /// </summary>
        [Required]
        [MaxLength(20)]
        string ShortName { get; set; }
    }
}

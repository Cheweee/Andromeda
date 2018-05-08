using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.Interfaces
{
    /// <summary>
    /// Name of the entity interface
    /// </summary>
    public interface INameEntity
    {
        /// <summary>
        /// Name of the entity (required property)
        /// </summary>
        [Required]
        [MaxLength(255)]
        string Name { get; set; }
    }
}

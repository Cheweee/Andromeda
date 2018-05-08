using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.Interfaces
{
    /// <summary>
    /// Key of the entity interface
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IKeyEntity<TKey>
    {
        /// <summary>
        /// Key of the entity
        /// </summary>
        [Key]
        TKey Id { get; set; }
    }
}

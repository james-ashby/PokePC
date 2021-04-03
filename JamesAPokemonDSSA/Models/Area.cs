using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JamesAPokemonWAD.Models
{
    public class Area
    {
        [Key]
        public int AreaId { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }
        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Image { get; set; }
        [Required]
        public int ExpPerCatch { get; set; }
        [Required]
        public int LevelRequirement { get; set; }

        public virtual ICollection<AreasPokemon> AreasPokemon { get; set; }

    }
}

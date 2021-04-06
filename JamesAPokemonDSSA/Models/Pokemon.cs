using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JamesAPokemonWAD.Models
{
    public class Pokemon
    {
        [Key]
        public int PokemonId { get; set; }

        [Required]
        [Column(TypeName = "varchar(30)")]
        public string PokemonName { get; set; }

        [Required]
        public int PokedexNum { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Type_1 { get; set; }

        [Column(TypeName = "varchar(50)")]          // Not required since some Pokémon do not have a secondary type
        public string Type_2 { get; set; }

        [Required]
        [Column(TypeName = "varchar(60)")]
        public string Classification { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Height { get; set; }      // Height in metres

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Weight { get; set; }      // Weight in kilograms

        [Required]
        [Column(TypeName = "decimal(10)")]
        public decimal Generation { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Image { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string ShinyImage { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Rarity { get; set; }

        public virtual ICollection<AreasPokemon> AreasPokemon { get; set; }
    }
}

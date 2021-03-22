using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JamesAPokemonWAD.Models
{
    public class DexEntry
    {
        [Key]
        public int DexID { get; set; }

        [Required]
        [Column(TypeName = "varchar(30)")]
        public string DexName { get; set; }


        [Required]
        [Column(TypeName = "varchar(50)")]
        public string DexType_1 { get; set; }

        [Column(TypeName = "varchar(50)")]          // Not required since some Pokémon do not have a secondary type
        public string DexType_2 { get; set; }

        [Required]
        [Column(TypeName = "varchar(60)")]
        public string DexClassification { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string DexDescription { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string DexImage { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string DexShinyImage { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal DexHeight { get; set; }      // Height in metres

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal DexWeight { get; set; }      // Weight in kilograms

        [Required]
        [Column(TypeName = "decimal(10)")]
        public decimal DexGeneration { get; set; }
    }
}

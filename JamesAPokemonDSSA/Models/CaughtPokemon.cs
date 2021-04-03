using JamesAPokemonWAD.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JamesAPokemonWAD.Models
{
    public class CaughtPokemon
    {
        [Key]
        public int PokemonID { get; set; }


        [Column(TypeName = "nvarchar(450)")]
        public string UserID { get; set; }

        public string PokemonName { get; set; }
        public bool IsShiny { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CatchDate { get; set; }

        [Column(TypeName = "varchar(15)")]
        public string Nickname { get; set; }
    }
}

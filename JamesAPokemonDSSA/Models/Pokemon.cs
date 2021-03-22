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
        public int PokeID { get; set; }

        public int UserID { get; set; }

        public bool IsShiny { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CatchDate { get; set; }

        [Column(TypeName = "varchar(15)")]
        public string Nickname { get; set; }
    }
}

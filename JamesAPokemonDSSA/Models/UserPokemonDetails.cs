using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JamesAPokemonWAD.Models
{
    public class UserPokemonDetails
    {
        public int PokemonId { get; set; }
        public string Name { get; set; }
        public bool IsShiny { get; set; }
        public DateTime CatchDate { get; set; }
        public string Nickname { get; set; }
        public int PokedexNum { get; set; }
        public string Image { get; set; }
        
        public string Type_1 { get; set; }
        public string Type_2 { get; set; }
        public string queryString { get; set; }
    }
}

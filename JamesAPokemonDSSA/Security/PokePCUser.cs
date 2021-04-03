using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JamesAPokemonWAD.Security
{
    public class PokePCUser : IdentityUser
    {
        public int UniquePokemon { get; set; }

        public int Money { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }

        public int Level { get; set; }

        public int Experience { get; set; }
    }
}

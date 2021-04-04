using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JamesAPokemonWAD.Models
{
    public class AccountDetails
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public int TotalPokemon { get; set; }

        public int TrainerLevel { get; set; }

        public int Experience { get; set; }
        public string AccountType { get; set; }

        public string Created { get; set; }


    }
}

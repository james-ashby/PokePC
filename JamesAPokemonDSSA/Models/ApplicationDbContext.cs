using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JamesAPokemonWAD.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<DexEntry> Pokedex { get; set; }

        public DbSet<Pokemon> AllPokemon { get; set; }

    }
}

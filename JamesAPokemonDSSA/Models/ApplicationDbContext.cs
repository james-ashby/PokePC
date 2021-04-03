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
        public DbSet<Pokemon> Pokemon { get; set; }

        public DbSet<CaughtPokemon> CaughtPokemon { get; set; }

        public DbSet<Area> Areas { get; set; }

        public DbSet<AreasPokemon> AreaPokemon { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AreasPokemon>().HasKey(x => new { x.AreaId, x.PokemonId });

            modelBuilder.Entity<AreasPokemon>().HasOne(p => p.Area).WithMany(x => x.AreasPokemon).HasForeignKey(t => t.AreaId);
            modelBuilder.Entity<AreasPokemon>().HasOne(p => p.Pokemon).WithMany(x => x.AreasPokemon).HasForeignKey(t => t.PokemonId);

        }
    }
}

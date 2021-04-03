using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JamesAPokemonWAD.Security
{
    public class AppIdentityDbContext : IdentityDbContext<PokePCUser, PokePCRoles, string>
    {
        public AppIdentityDbContext (DbContextOptions<AppIdentityDbContext> options) : base(options)
        {

        }
    }
}

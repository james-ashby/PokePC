using JamesAPokemonDSSA.Models;
using JamesAPokemonWAD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace JamesAPokemonDSSA.Controllers
{
    public class PokePCController : Controller
    {
        private readonly ILogger<PokePCController> _logger;
        private readonly ApplicationDbContext _context;
        public PokePCController(ILogger<PokePCController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
           var starters = _context.Pokemon.Where(e =>(e.PokedexNum == 1 || e.PokedexNum == 4 || e.PokedexNum == 7));
           List < Pokemon > model = starters.ToList();
            return View(model);
        }

        public IActionResult Pokedex()
        {
            var allPokemon = _context.Pokemon.OrderBy(e => e.PokedexNum);
            List<Pokemon> model = allPokemon.ToList();
            return View(model);
        }
        public IActionResult Areas()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

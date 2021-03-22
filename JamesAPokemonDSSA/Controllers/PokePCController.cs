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
            var starters = _context.Pokedex.Where(e =>(e.DexID == 1 || e.DexID == 4 || e.DexID == 7));
            List < DexEntry > model = starters.ToList();
            return View(model);
        }

        public IActionResult Pokedex()
        {
            return View(_context.Pokedex.ToList());
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

using JamesAPokemonDSSA.Models;
using JamesAPokemonWAD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using JamesAPokemonWAD.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace JamesAPokemonDSSA.Controllers
{
    public class PokePCController : Controller
    {
        private readonly ILogger<PokePCController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<PokePCUser> _userManager;
        private readonly AppIdentityDbContext _userContext;
        public PokePCController(ILogger<PokePCController> logger, ApplicationDbContext context, UserManager<PokePCUser>userManager, AppIdentityDbContext userContext)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _userContext = userContext;
        }

        public IActionResult Index()
        {
           var starters = _context.Pokemon.Where(e =>(e.PokedexNum == 1 || e.PokedexNum == 4 || e.PokedexNum == 7));
           List < Pokemon > model = starters.ToList();
            return View(model);
        }

        public async Task<IActionResult> Pokedex(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            var allPokemon = _context.Pokemon.OrderBy(e => e.PokedexNum);
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["TypeSortParm"] = sortOrder;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var pokemon = from p in _context.Pokemon select p;
            if (!string.IsNullOrEmpty(searchString))
            {
                pokemon = pokemon.Where(p => p.PokemonName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    pokemon = pokemon.OrderByDescending(p => p.PokemonName);
                    break;
                case "name_asc":
                    pokemon = pokemon.OrderBy(p => p.PokemonName);
                    break;
                case "Date":
                    pokemon = pokemon.OrderBy(p => p.PokemonId);
                    break;
                case "date_desc":
                    pokemon = pokemon.OrderByDescending(p => p.PokemonId);
                    break;
                default:
                    pokemon = pokemon.OrderBy(p => p.PokedexNum);
                    break;
            }
            int pageSize = 14;
            ViewData["Results"] = pokemon.Count();
            ViewData["Pages"] = Math.Floor((decimal)pokemon.Count() / 14) + 1;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["Types"] = _context.Pokemon.Select(p => p.Type_1).Distinct().ToList();
            return View(await PaginatedList<Pokemon>.CreateAsync(pokemon.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        public IActionResult PokedexEntry(int id)
        {
            Pokemon model = _context.Pokemon.Find(id);
            return View(model);
        }
        public IActionResult Areas()
        {
            List<Area> areas = _context.Areas.OrderBy(a => a.LevelRequirement).ToList();
            if (User.Identity.IsAuthenticated)
            {
                ViewData["UserLevel"] = _userContext.Users.Find(_userManager.GetUserId(User)).Level;
            }
            else
            {
                ViewData["UserLevel"] = -1;
            }
            return View(areas);
        }
        public IActionResult AreaDetails(int id)
        {
            var area = _context.Areas.Find(id);
            var levelReq = area.LevelRequirement;
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Denied"] = "You need to be logged in to find Pokémon!";
                return RedirectToAction("Areas", "PokePC");
            }else if (_userContext.Users.Find(_userManager.GetUserId(User)).Level < levelReq)
            {
                TempData["Denied"] = area.Name + " requires level " + levelReq;
                return RedirectToAction("Areas", "PokePC");
            }

            List<AreasPokemon> areaPoke = _context.AreaPokemon.Include(p => p.Pokemon).Where(a => a.AreaId == id).OrderBy(p => p.Rarity == "Legendary")
            .ThenBy(p => p.Rarity == "Rare").ThenBy(p => p.Rarity == "Uncommon").ThenBy(p => p.Rarity == "Common").ToList();
            ViewData["AreaImage"] = _context.Areas.Where(a => a.AreaId == id).First().Image;
            ViewData["AreaName"] = _context.Areas.Find(id).Name;
            return View(areaPoke);
        }
        [Authorize(Roles = "Admin, Standard")]
        public IActionResult Area(int id)
        {
            var area = _context.Areas.Find(id);
            ViewData["AreaName"] = area.Name;
            var levelReq = area.LevelRequirement;
            if (_userContext.Users.Find(_userManager.GetUserId(User)).Level < levelReq)
            {
                TempData["Denied"] = area.Name + " requires level " + levelReq;
                return RedirectToAction("Areas", "PokePC");
            }
            Random rand = new Random(DateTime.Now.Millisecond);
            var roll = rand.Next(1, 101);
            var shinyRoll = rand.Next(1, 150);
            var model = _context.AreaPokemon.Include(c => c.Pokemon).Where(a=> a.AreaId == id);
            List<AreasPokemon> areaPoke = model.ToList();
            ViewData["IsShiny"] = (shinyRoll == 1 ? true : false);
            AreasPokemon legendaryPoke = areaPoke.OrderBy(c => Guid.NewGuid()).Where(p => p.Rarity == "Legendary").FirstOrDefault(); // Guid for simple random choice (random global unique identifier)
            AreasPokemon rarePoke = areaPoke.OrderBy(c => Guid.NewGuid()).Where(p => p.Rarity == "Rare").FirstOrDefault();
            AreasPokemon uncommonPoke = areaPoke.OrderBy(c => Guid.NewGuid()).Where(p => p.Rarity == "Uncommon").FirstOrDefault();
            AreasPokemon commonPoke = areaPoke.OrderBy(c => Guid.NewGuid()).Where(p => p.Rarity == "Common").FirstOrDefault();
            if (roll == 1 && legendaryPoke != null)
            {
                if (_context.CaughtPokemon.Where(u => u.UserID == _userManager.GetUserId(User) && u.PokemonName == legendaryPoke.Pokemon.PokemonName).FirstOrDefault() != null)
                {
                    TempData["Caught"] = true;
                }
                return View(legendaryPoke);
            }
            else if (roll <= 5 && rarePoke != null)
            {
                if (_context.CaughtPokemon.Where(u => u.UserID == _userManager.GetUserId(User) && u.PokemonName == rarePoke.Pokemon.PokemonName).FirstOrDefault() != null)
                {
                    TempData["Caught"] = true;
                }
                return View(rarePoke);
            }
            else if (roll <= 20 && uncommonPoke != null)
            {
                if (_context.CaughtPokemon.Where(u => u.UserID == _userManager.GetUserId(User) && u.PokemonName == uncommonPoke.Pokemon.PokemonName).FirstOrDefault() != null)
                {
                    TempData["Caught"] = true;
                }
                return View(uncommonPoke);
            }
            else if (roll <= 70 && commonPoke != null)
            {
                if (_context.CaughtPokemon.Where(u => u.UserID == _userManager.GetUserId(User) && u.PokemonName == commonPoke.Pokemon.PokemonName).FirstOrDefault() != null)
                {
                    TempData["Caught"] = true;
                }
                return View(commonPoke);
            }
            else
            {
                ViewData["AreaId"] = id;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> CaughtPokemon(int id, bool isShiny)
        {
            Pokemon model = _context.Pokemon.Find(id);
            CaughtPokemon caught = new CaughtPokemon
            {
                UserID = _userManager.GetUserId(User),
                PokemonName = model.PokemonName,
                IsShiny = isShiny,
                CatchDate = DateTime.UtcNow,
            };

            PokePCUser user = _userContext.Users.Find(_userManager.GetUserId(User));
            ViewData["PrevExperience"] = user.Experience;
            var levelup = false;
            user.Experience = user.Experience + 100;
            if (user.Experience == user.Level * 1500)
            {
                user.Level = user.Level + 1;
                levelup = true;
            }
            user.UniquePokemon = user.UniquePokemon + 1;
            _userContext.SaveChanges();
            _context.Add(caught);
            _context.SaveChanges();
            ViewData["IsShiny"] = isShiny;
            ViewData["NewExperience"] = user.Experience;
            ViewData["LevelUp"] = levelup;
            ViewData["Level"] = user.Level;
            return PartialView("_CaughtPokemon", model);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}

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
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        public async Task<IActionResult> Index()
        {
            List<Pokemon> model = await _context.Pokemon.Where(e => (e.PokedexNum == 1 || e.PokedexNum == 4 || e.PokedexNum == 7)).ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> Pokedex(string sortOrder, string searchString, string currentFilter, int? pageNumber, string typeFilter, string currentType)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null || typeFilter != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
                typeFilter = currentType;
            }
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentType"] = typeFilter;

             var pokemon = from p in _context.Pokemon select p;
            if (typeFilter != null)
            {
                pokemon = pokemon.Where(p => p.Type_1 == typeFilter || p.Type_2 == typeFilter);
            }
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
        public async Task<IActionResult> PokedexEntry(int id)
        {
            Pokemon model = _context.Pokemon.Find(id);
            try
            {
                // Connects to the Pokémon API and retrieves the official artwork (if available) for the Pokémon number selected
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync($"https://pokeapi.co/api/v2/pokemon/" + model.PokedexNum))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        JObject pokemonApiInfo = JObject.Parse(apiResponse);
                        ViewBag.apiData = new Dictionary<string, string>
                    {
                        {"art", pokemonApiInfo["sprites"]["other"]["official-artwork"]["front_default"].ToString() },
                    };
                    }
                }
            }
            catch (Exception)
            {
                return View(model);
            }

            
            return View(model);
        }
        public async Task<IActionResult> Areas()
        {
            List<Area> areas = await _context.Areas.OrderBy(a => a.LevelRequirement).ToListAsync();
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
        public async Task<IActionResult> AreaDetails(int id)
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


            // Order the Pokémon by rarity on the Area Details page
            List<AreasPokemon> areaPoke = await _context.AreaPokemon.Include(p => p.Pokemon).Where(a => a.AreaId == id).OrderBy(p => p.Pokemon.Rarity == "Legendary")
            .ThenBy(p => p.Pokemon.Rarity == "Rare").ThenBy(p => p.Pokemon.Rarity == "Uncommon").ThenBy(p => p.Pokemon.Rarity == "Common").ToListAsync();
            if (areaPoke.Count == 0)
            {
                TempData["Error"] = "Area under construction, no wild Pokémon to be found here for now!";
                return RedirectToAction("Areas", "PokePC");
            }
            string imageUrl = _context.Areas.Where(a => a.AreaId == id).FirstAsync().Result.Image;
            ViewData["AreaImage"] = imageUrl.Contains("uploaded") ? "/images/areas/" + imageUrl : imageUrl;
            ViewData["AreaName"] = _context.Areas.Find(id).Name;
            return View(areaPoke);
        }
        [Authorize(Roles = "Admin, Standard")]
        public async Task<IActionResult> Area(int id)
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
            // Roll a number between 1 and 1000 to calculate rarity for Wild Pokémon <= 5 for Legendary, <=60 for Rare, <= 200 for Uncommon, <= 700 for Common, with the remaining 300 for no Pokémon appeared
            var roll = rand.Next(1, 1001);
            var shinyRoll = rand.Next(1, 150); 
            List<AreasPokemon> areaPoke = await _context.AreaPokemon.Include(c => c.Pokemon).Where(a => a.AreaId == id).ToListAsync();
            ViewData["IsShiny"] = (shinyRoll == 1 ? true : false);
            AreasPokemon rolledPoke = null;
            if (roll <= 5)
            {
                rolledPoke = areaPoke.OrderBy(c => Guid.NewGuid()).Where(p => p.Pokemon.Rarity == "Legendary").FirstOrDefault();
            }
            else if (roll <= 60)
            {
                rolledPoke = areaPoke.OrderBy(c => Guid.NewGuid()).Where(p => p.Pokemon.Rarity == "Rare").FirstOrDefault();
            }
            else if (roll <= 200)
            {
                rolledPoke = areaPoke.OrderBy(c => Guid.NewGuid()).Where(p => p.Pokemon.Rarity == "Uncommon").FirstOrDefault();
            }
            else if (roll <= 700)
            {
                
                rolledPoke = areaPoke.OrderBy(c => Guid.NewGuid()).Where(p => p.Pokemon.Rarity == "Common").FirstOrDefault();
            }
            if (rolledPoke == null)
            {
                ViewData["AreaId"] = id;
                return View();
            }
            else
            {
                if (_context.CaughtPokemon.Where(u => u.UserID == _userManager.GetUserId(User) && u.PokemonName == rolledPoke.Pokemon.PokemonName).FirstOrDefault() != null)
                {
                    // Send temporary data of whether the user has already caught the Pokémon before, if they have, show a Pokéball next to the name
                    TempData["Caught"] = true;
                }
                try
                {
                    // Connect to the Pokémon api and retrieve a gif for the Pokémon that is found, if an animated gif for the Pokémon is found, use that for the wild encounter
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.GetAsync($"https://pokeapi.co/api/v2/pokemon/" + rolledPoke.Pokemon.PokemonName.ToLower()))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            JObject pokemonApiInfo = JObject.Parse(apiResponse);
                            ViewBag.apiData = new Dictionary<string, string>
                        {
                            {"gif", pokemonApiInfo["sprites"]["versions"]["generation-v"]["black-white"]["animated"]["front_default"].ToString() },
                            {"gifShiny", pokemonApiInfo["sprites"]["versions"]["generation-v"]["black-white"]["animated"]["front_shiny"].ToString() }
                        };
                        }
                    }
                }
                catch (Exception)
                {
                    return View(rolledPoke);
                }

                return View(rolledPoke);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CaughtPokemon(int id, bool isShiny, int areaId)
        {
            Pokemon model = await _context.Pokemon.FindAsync(id);
            CaughtPokemon caught = new CaughtPokemon
            {
                UserID = _userManager.GetUserId(User),
                PokemonName = model.PokemonName,
                IsShiny = isShiny,
                CatchDate = DateTime.UtcNow,
            };

            PokePCUser user = await _userContext.Users.FindAsync(_userManager.GetUserId(User));
            ViewData["PrevExperience"] = user.Experience;
            var levelup = false;
            int areaExp = _context.Areas.Find(areaId).ExpPerCatch;
            // Calculate the experience that the player's next level requires
            double nextLvlExp = (user.Level * 1000) * (1.5);

            // Add the experience from catching the Pokémon
            user.Experience = user.Experience + areaExp;

            // Calculate the experience the user needs to level up // Ternary operator to account for if user experience is over the next level experience, which sets required exp to 0
            double expToLvl = nextLvlExp - user.Experience < 0 ? 0 : nextLvlExp - user.Experience;

            // If the user's experience exceeds the amount required for their next level, level them up by 1
            if (user.Experience >= nextLvlExp)
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
            ViewData["ExpGain"] = areaExp;
            ViewData["ExpLeft"] = expToLvl;
            return PartialView("_CaughtPokemon", model);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}

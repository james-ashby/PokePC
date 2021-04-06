using JamesAPokemonWAD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JamesAPokemonWAD.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webEnvironment;
        public AdminController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {

            _context = context;
            webEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Pokemon(string sortOrder, string searchString, string currentFilter, int? pageNumber)
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
            int pageSize = 16;
            ViewData["Results"] = pokemon.Count();
            ViewData["Pages"] = Math.Floor((decimal)pokemon.Count() / 16) + 1;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["Types"] = _context.Pokemon.Select(p => p.Type_1).Distinct().ToList();
            return View(await PaginatedList<Pokemon>.CreateAsync(pokemon.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        public IActionResult Areas()
        {
            List<Area> allAreas = _context.Areas.ToList();
            return View(allAreas);
        }
        public IActionResult AddArea()
        {
            ViewData["Pokemon"] = _context.Pokemon.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddArea(AreaForm model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string imageUrl = UpdatedAreaImage(model);
                    Area newArea = new Area
                    {
                        Name = model.AreaName,
                        ExpPerCatch = model.ExpPerCatch,
                        LevelRequirement = model.LevelRequirement,
                        Image = imageUrl

                    };
                    _context.Add(newArea);
                    _context.SaveChanges();
                    List<AreasPokemon> wildPokemon = new List<AreasPokemon>();
                    _context.AreaPokemon.Where(ap => ap.AreaId == newArea.AreaId).ToList().ForEach(a => _context.AreaPokemon.Remove(a));
                    if (model.PokemonIds != null)
                    {
                        foreach (int pokemonId in model.PokemonIds)
                        {
                            wildPokemon.Add(new AreasPokemon { AreaId = model.AreaId, PokemonId = pokemonId });
                        }
                        foreach (AreasPokemon wildPoke in wildPokemon)
                        {
                            _context.Add(wildPoke);
                        }
                    }
                    _context.SaveChanges();
                    return RedirectToAction("Areas", "Admin");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Custom", "Area name already exists!");
                    return View(model);
                }
            }

            return View(model);
        }
        public IActionResult UpdateArea(int id)
        {
            Area area = _context.Areas.Find(id);
            var SelectedPokes = _context.AreaPokemon.Where(p => p.AreaId== id).Select(a => a.PokemonId).ToList();
            List<Pokemon> allPokemon = _context.Pokemon.ToList();
            AreaForm model = new AreaForm
            {
                AreaId = area.AreaId,
                AreaName = area.Name,
                ExpPerCatch = area.ExpPerCatch,
                ImageUrl = area.Image,
                LevelRequirement = area.LevelRequirement,
                AllPoke = allPokemon,
                PokemonIds = SelectedPokes
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateArea(AreaForm model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string imageUrl = UpdatedAreaImage(model);
                    Area area = _context.Areas.Find(model.AreaId);
                    string filepath = Path.Combine(webEnvironment.WebRootPath, "images/areas");
                    if (imageUrl != null)
                    {
                        var areaImgPath = Path.Combine(Directory.GetCurrentDirectory(), filepath, area.Image);
                        if (System.IO.File.Exists(areaImgPath))
                        {
                            System.IO.File.Delete(areaImgPath);
                        }
                    }
                    area.Name = model.AreaName;
                    area.ExpPerCatch = model.ExpPerCatch;
                    area.LevelRequirement = model.LevelRequirement;
                    area.Image = imageUrl == null ? area.Image : imageUrl;
                    List<AreasPokemon> wildPokemon = new List<AreasPokemon>();
                    _context.AreaPokemon.Where(ap => ap.AreaId == area.AreaId).ToList().ForEach(a => _context.AreaPokemon.Remove(a));
                    if (model.PokemonIds != null)
                    {
                        foreach (int pokemonId in model.PokemonIds)
                        {
                            wildPokemon.Add(new AreasPokemon { AreaId = model.AreaId, PokemonId = pokemonId});
                        }
                        foreach (AreasPokemon wildPoke in wildPokemon)
                        {
                            _context.Add(wildPoke);
                        }
                    }
                    _context.SaveChanges();
                    return RedirectToAction("Areas", "Admin");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Custom", "Area name already exists!");
                    return View(model);
                }
            }

            return View(model);
        }
        public IActionResult AddPokemon()
        {
            ViewData["Types"] = _context.Pokemon.Select(p => p.Type_1).Distinct().ToList();
            ViewData["Areas"] = _context.Areas.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPokemon(AddPokemon model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string[] imageUrls = UploadedPokemonImages(model);
                    string standardUrl = imageUrls[0];
                    string shinyImageUrl = imageUrls[1];
                    
                    Pokemon newPokemon = new Pokemon
                    {
                        PokemonName = model.PokemonName,
                        PokedexNum = model.PokedexNum,
                        Type_1 = model.Type_1,
                        Type_2 = model.Type_2,
                        Classification = model.Classification,
                        Description = model.Description,
                        Height = model.Height,
                        Weight = model.Weight,
                        Generation = model.Generation,
                        Image = standardUrl,
                        ShinyImage = shinyImageUrl,
                        Rarity = model.Rarity
                    };
                    _context.Add(newPokemon);
                    _context.SaveChanges();
                    List<AreasPokemon> newListOfAreas = new List<AreasPokemon>();
                    if (model.AreaIds != null)
                    {
                        foreach (int areaId in model.AreaIds)
                        {
                            newListOfAreas.Add(new AreasPokemon { AreaId = areaId, PokemonId = newPokemon.PokemonId});
                        }
                        foreach (AreasPokemon area in newListOfAreas)
                        {
                            _context.Add(area);
                            _context.SaveChanges();
                        }
                    }
                    return RedirectToAction("Pokemon", "Admin");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Custom", "Pokémon Name or Number already exists!");
                    ViewData["Types"] = _context.Pokemon.Select(p => p.Type_1).Distinct().ToList();
                    ViewData["Areas"] = _context.Areas.ToList();
                    return View(model);
                }
                
            }
            ViewData["Types"] = _context.Pokemon.Select(p => p.Type_1).Distinct().ToList();
            ViewData["Areas"] = _context.Areas.ToList();
            return View(model);
        }
        public IActionResult UpdatePokemon(int id)
        {
            Pokemon model = _context.Pokemon.Find(id);
            var areaIDs = _context.AreaPokemon.Where(p => p.PokemonId == id).Select(a => a.AreaId).ToList();
            List<Area> allAreas = _context.Areas.ToList();
            UpdatePokemon formModel = new UpdatePokemon
            {
                PokemonId = model.PokemonId,
                PokemonName = model.PokemonName,
                PokedexNum = model.PokedexNum,
                Type_1 = model.Type_1,
                Type_2 = model.Type_2,
                Classification = model.Classification,
                Description = model.Description,
                Height = model.Height,
                Weight = model.Weight,
                Generation = model.Generation,
                ImageUrl = model.Image,
                SelectedAreas = allAreas,
                AreaIds = areaIDs,
                Rarity = model.Rarity
            };
            ViewData["Types"] = _context.Pokemon.Select(p => p.Type_1).Distinct().ToList();
            ViewData["Areas"] = _context.Areas.Select(a => a.AreaId).ToList();
            return View(formModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePokemon (UpdatePokemon model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string[] imageUrls = UpdatedPokemonImages(model);
                    string standardUrl = imageUrls[0];
                    string shinyImageUrl = imageUrls[1];
                    Pokemon pokemon = _context.Pokemon.Find(model.PokemonId);
                    string filepath = Path.Combine(webEnvironment.WebRootPath, "images/pokemon");
                    if (standardUrl != null)
                    {
                        var standardPath = Path.Combine(Directory.GetCurrentDirectory(), filepath, pokemon.Image);
                        if (System.IO.File.Exists(standardPath))
                        {
                            System.IO.File.Delete(standardPath);
                        }
                    }
                    if (shinyImageUrl != null)
                    {
                        var shinyPath = Path.Combine(Directory.GetCurrentDirectory(), filepath, pokemon.ShinyImage);
                        if (System.IO.File.Exists(shinyPath))
                        {
                            System.IO.File.Delete(shinyPath);
                        }
                    }
                    pokemon.PokemonName = model.PokemonName;  
                    pokemon.PokedexNum = model.PokedexNum;  
                    pokemon.Type_1 = model.Type_1;  
                    pokemon.Type_2 = model.Type_2;
                    pokemon.Classification = model.Classification;
                    pokemon.Description = model.Description;
                    pokemon.Height = model.Height;
                    pokemon.Weight = model.Weight;
                    pokemon.Generation = model.Generation;
                    pokemon.Image = standardUrl == null ? pokemon.Image : standardUrl;
                    pokemon.ShinyImage = shinyImageUrl == null ? pokemon.ShinyImage : shinyImageUrl;
                    pokemon.Rarity = model.Rarity;
                    // Removes areas that have been deselected
                    _context.AreaPokemon.Where(ap => ap.PokemonId == pokemon.PokemonId).ToList().ForEach(a => _context.AreaPokemon.Remove(a));
                    // Adds areas the have been selected
                    List<AreasPokemon> newListOfAreas = new List<AreasPokemon>();
                    if (model.AreaIds != null)
                    {
                        foreach (int areaId in model.AreaIds)
                        {
                            newListOfAreas.Add(new AreasPokemon { AreaId = areaId, PokemonId = pokemon.PokemonId});
                        }
                        foreach (AreasPokemon area in newListOfAreas)
                        {
                            _context.Add(area);
                        }
                    }
                    _context.SaveChanges();
                    return RedirectToAction("Pokemon", "Admin");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Custom", "Pokémon Name or Number already exists!");
                    ViewData["Types"] = _context.Pokemon.Select(p => p.Type_1).Distinct().ToList();
                    ViewData["Areas"] = _context.Areas.Select(a => a.Name).ToList();
                    return View(model);
                }
                
            }
            ViewData["Types"] = _context.Pokemon.Select(p => p.Type_1).Distinct().ToList();
            ViewData["Areas"] = _context.Areas.Select(a => a.Name).ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult ConfirmDeletePokemon(int id)
        {
            Pokemon model = _context.Pokemon.Find(id);
            return PartialView("_ConfirmDeletePokemon", model);
        }
        [HttpPost]
        public IActionResult DeletePokemon(Pokemon id)
        {
            Pokemon pokemon = _context.Pokemon.Find(id.PokemonId);
            string filepath = Path.Combine(webEnvironment.WebRootPath, "images/pokemon");
            var standardPath = Path.Combine(Directory.GetCurrentDirectory(), filepath, pokemon.Image);
            var shinyPath = Path.Combine(Directory.GetCurrentDirectory(), filepath, pokemon.ShinyImage);

            if (System.IO.File.Exists(standardPath) && System.IO.File.Exists(shinyPath))
            {
                System.IO.File.Delete(standardPath);
                System.IO.File.Delete(shinyPath);
            }
            _context.Pokemon.Remove(pokemon);
            _context.SaveChanges();
            return RedirectToAction("Pokemon", "Admin");
        }
        [HttpPost]
        public IActionResult ConfirmDeleteArea(int id)
        {
            Area model = _context.Areas.Find(id);
            return PartialView("_ConfirmDeleteArea", model);
        }
        [HttpPost]
        public IActionResult DeleteArea(Area id)
        {
            Area area = _context.Areas.Find(id.AreaId);
            string filepath = Path.Combine(webEnvironment.WebRootPath, "images/areas");
            var areaImgPath = Path.Combine(Directory.GetCurrentDirectory(), filepath, area.Image);

            if (System.IO.File.Exists(areaImgPath))
            {
                System.IO.File.Delete(areaImgPath);
            }
            _context.Areas.Remove(area);
            _context.SaveChanges();
            return RedirectToAction("Areas", "Admin");
        }
        private string[] UploadedPokemonImages(AddPokemon model)
        {
            string[] imageUrls = new string[2];
            
            if (model.UploadImage != null && model.UploadShinyImage != null)
            {
                string uploadsFolder = Path.Combine(webEnvironment.WebRootPath, "images/pokemon");
                imageUrls[0] = "uploaded" + Guid.NewGuid().ToString() + "_" + model.UploadImage.FileName;
                imageUrls[1] = "uploaded" + Guid.NewGuid().ToString() + "_" + model.UploadShinyImage.FileName;
                string imagePath = Path.Combine(uploadsFolder, imageUrls[0]);
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    model.UploadImage.CopyTo(fileStream);
                }
                string shinyImagePath = Path.Combine(uploadsFolder, imageUrls[1]);
                using (var fileStream = new FileStream(shinyImagePath, FileMode.Create))
                {
                    model.UploadShinyImage.CopyTo(fileStream);
                }
            }
            return imageUrls;
        }
        private string[] UpdatedPokemonImages(UpdatePokemon model)
        {
            string[] imageUrls = new string[2];

            if (model.UploadImage != null && model.UploadShinyImage != null)
            {
                string uploadsFolder = Path.Combine(webEnvironment.WebRootPath, "images/pokemon");
                imageUrls[0] = "uploaded" + Guid.NewGuid().ToString() + "_" + model.UploadImage.FileName;
                imageUrls[1] = "uploaded" + Guid.NewGuid().ToString() + "_" + model.UploadShinyImage.FileName;
                string imagePath = Path.Combine(uploadsFolder, imageUrls[0]);
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    model.UploadImage.CopyTo(fileStream);
                }
                string shinyImagePath = Path.Combine(uploadsFolder, imageUrls[1]);
                using (var fileStream = new FileStream(shinyImagePath, FileMode.Create))
                {
                    model.UploadShinyImage.CopyTo(fileStream);
                }
            }
            return imageUrls;
        }
        private string UpdatedAreaImage(AreaForm model)
        {
            string imageUrl = null;
            if (model.UploadImage != null)
            {
                string uploadsFolder = Path.Combine(webEnvironment.WebRootPath, "images/areas");
                imageUrl = "uploaded" + Guid.NewGuid().ToString() + "_" + model.UploadImage.FileName;
                string imagePath = Path.Combine(uploadsFolder, imageUrl);
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    model.UploadImage.CopyTo(fileStream);
                }
            }
            return imageUrl;
        }
    }
}

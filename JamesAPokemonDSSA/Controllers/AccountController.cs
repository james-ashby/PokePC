using JamesAPokemonWAD.Models;
using JamesAPokemonWAD.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Routing;

namespace JamesAPokemonDSSA.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<PokePCUser> userManager;
        private readonly RoleManager<PokePCRoles> roleManager;
        private readonly SignInManager<PokePCUser> signinManager;
        private readonly ApplicationDbContext _context;
        private readonly AppIdentityDbContext _userContext;
        public AccountController(UserManager<PokePCUser> userManager, RoleManager<PokePCRoles> roleManager, SignInManager<PokePCUser> signinManager, ApplicationDbContext context, AppIdentityDbContext userContext)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signinManager = signinManager;
            _context = context;
            _userContext = userContext;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Login obj)
        {
            if (ModelState.IsValid)
            {
                var result = signinManager.PasswordSignInAsync(obj.UserName, obj.Password, obj.RememberMe, false).Result;
                if (result.Succeeded)
                {
                    HttpContext.Items["Username"] = obj.UserName;
                    return RedirectToAction("Index", "PokePC");
                }
                else
                {
                    ModelState.AddModelError("LoginFailed", "Invalid login details");
                }
            }
            return View(obj);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            signinManager.SignOutAsync().Wait();
            return RedirectToAction("Index", "PokePC");
        }
        public async Task<IActionResult> Register()
        {
            ViewData["Starters"] = await _context.Pokemon.Where(e => (e.PokedexNum == 1 || e.PokedexNum == 4 || e.PokedexNum == 7)).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register obj)
        {
            if (ModelState.IsValid)
            {
                if (!roleManager.RoleExistsAsync("Standard").Result)
                {
                    PokePCRoles role = new PokePCRoles();
                    role.Name = "Standard";
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
                PokePCUser user = new PokePCUser();
                user.UserName = obj.UserName;
                user.Email = obj.Email;
                user.CreationDate = DateTime.UtcNow;
                user.UniquePokemon = 1;
                user.Money = 1000;
                user.Level = 1;
                user.Experience = 0;
                IdentityResult result = userManager.CreateAsync(user, obj.Password).Result;            
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Standard").Wait();

                    CaughtPokemon starter = new CaughtPokemon
                    {
                        UserID = user.Id,
                        PokemonName = obj.Starter,
                        IsShiny = false,
                        CatchDate = DateTime.UtcNow,
                    };
                    _context.Add(starter);
                    _context.SaveChanges();
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid user details");
                }
            }
            ViewData["Starters"] = await _context.Pokemon.Where(e => (e.PokedexNum == 1 || e.PokedexNum == 4 || e.PokedexNum == 7)).ToListAsync();
            return View(obj);
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        [Authorize(Roles = "Standard, Admin")]
        public async Task<IActionResult> Details()
        {
            var user = _userContext.Users.Find(userManager.GetUserId(User));
            var userRoleID = await _userContext.UserRoles.Where(u => u.UserId == user.Id).Select(r => r.RoleId).FirstOrDefaultAsync(); ;
            string userRole = _userContext.Roles.Where(r => r.Id == userRoleID).Select(r => r.Name).FirstOrDefaultAsync().Result.ToString();
            AccountDetails model = new AccountDetails {
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email,
                TotalPokemon = user.UniquePokemon,
                TrainerLevel = user.Level,
                Experience = user.Experience,
                AccountType = userRole,
                Created = user.CreationDate.ToString("dd/MM/yyyy")
            };
            return View(model);
        }
        [Authorize(Roles = "Standard, Admin")]

        public IActionResult ChangePassword()
        {
            ViewData["UserId"] = userManager.GetUserId(User);
            ViewData["Successful"] = false;
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Standard, Admin")]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.UserId);
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var update = await userManager.ResetPasswordAsync(user, token, model.NewPassword);
                if (!update.Succeeded)
                {
                    ModelState.AddModelError("Custom", "Unexpected error occurred!");
                    ViewData["Successful"] = false;
                    return View();
                }
                else
                {
                    ViewData["Successful"] = true;
                    return View();
                }
            }
            else
            {
                ViewData["Successful"] = false;
                return View();
            }
        }
        [Authorize(Roles = "Standard, Admin")]
        public async Task<IActionResult> Pokemon(string sortOrder, string searchString, string currentFilter, int? pageNumber, string typeFilter, string currentType)
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

            // Join the User table and caught Pokémon table to display the correct Pokémon caught by the current user
            var caughtPokemon = _context.Pokemon.Join(_context.CaughtPokemon.Where(u => u.UserID == userManager.GetUserId(User)), pokemon => pokemon.PokemonName, caught => caught.PokemonName, (pokemon, caught) => new UserPokemonDetails
            {
                PokemonId = caught.PokemonID,
                CatchDate = caught.CatchDate,
                Image = caught.IsShiny ? pokemon.ShinyImage : pokemon.Image,
                Name = caught.PokemonName,
                PokedexNum = pokemon.PokedexNum,
                IsShiny = caught.IsShiny,
                Type_1 = pokemon.Type_1,
                Type_2 = pokemon.Type_2
            });

            if (typeFilter != null)
            {
                caughtPokemon = caughtPokemon.Where(p => p.Type_1 == typeFilter || p.Type_2 == typeFilter);
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                caughtPokemon = caughtPokemon.Where(p => p.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    caughtPokemon = caughtPokemon.OrderByDescending(p => p.Name);
                    break;
                case "name_asc":
                    caughtPokemon = caughtPokemon.OrderBy(p => p.Name);
                    break;
                case "Date":
                    caughtPokemon = caughtPokemon.OrderBy(p => p.CatchDate);
                    break;
                case "date_desc":
                    caughtPokemon = caughtPokemon.OrderByDescending(p => p.CatchDate);
                    break;
                default:
                    caughtPokemon = caughtPokemon.OrderBy(p => p.PokedexNum);
                    break;
            }
            int pageSize = 16;
            ViewData["Results"] = caughtPokemon.Count();
            ViewData["Pages"] = Math.Floor((decimal)caughtPokemon.Count() / 16) + 1;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["Types"] = _context.Pokemon.Select(p => p.Type_1).Distinct().ToList();
            ViewData["TotalPokemon"] = _userContext.Users.Find(userManager.GetUserId(User)).UniquePokemon;
            return View(await PaginatedList<UserPokemonDetails>.CreateAsync(caughtPokemon.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        [HttpPost]
        [Authorize(Roles = "Standard, Admin")]
        public async Task<IActionResult> ConfirmReleasePokemon(int id, string query)
        {
            // Join the Pokémon table to the Caught Pokémon table to create a model with more information, including date the Pokémon was captured
            UserPokemonDetails model = await _context.Pokemon.Join(_context.CaughtPokemon.Where(p => p.PokemonID == id), pokemon => pokemon.PokemonName, caught => caught.PokemonName, (pokemon, caught) => new UserPokemonDetails
            {
                PokemonId = caught.PokemonID,
                CatchDate = caught.CatchDate,
                Image = caught.IsShiny ? pokemon.ShinyImage : pokemon.Image,
                Name = caught.PokemonName,
                PokedexNum = pokemon.PokedexNum,
                IsShiny = caught.IsShiny,
                Type_1 = pokemon.Type_1,
                Type_2 = pokemon.Type_2,
                queryString = query
            }).FirstOrDefaultAsync();
            return PartialView("_ConfirmReleasePokemon", model);
        }
        [HttpPost]
        [Authorize(Roles = "Standard, Admin")]
        public IActionResult ReleasePokemon(UserPokemonDetails model)
        {
            CaughtPokemon pokemon = _context.CaughtPokemon.Find(model.PokemonId);
            var user = _userContext.Users.Find(userManager.GetUserId(User));
            user.UniquePokemon = user.UniquePokemon - 1;
            _context.CaughtPokemon.Remove(pokemon);
            _userContext.SaveChanges();
            _context.SaveChanges();
            return RedirectToAction("Pokemon", "Account");
        }
    }
}

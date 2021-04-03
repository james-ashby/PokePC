using JamesAPokemonWAD.Models;
using JamesAPokemonWAD.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                    ModelState.AddModelError("", "Invalid login details");
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
        public IActionResult Register()
        {
            ViewData["Starters"] = _context.Pokemon.Where(e => (e.PokedexNum == 1 || e.PokedexNum == 4 || e.PokedexNum == 7)).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Register obj)
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
            ViewData["Starters"] = _context.Pokemon.Where(e => (e.PokedexNum == 1 || e.PokedexNum == 4 || e.PokedexNum == 7)).ToList();
            return View(obj);
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult Details()
        {
            return View();
        }
        [Authorize(Roles = "Standard, Admin")]
        public IActionResult Pokemon()
        {

            List<UserPokemonDetails> caughtPokemon = _context.Pokemon.Join(_context.CaughtPokemon.Where(u => u.UserID == userManager.GetUserId(User)), pokemon => pokemon.PokemonName, caught => caught.PokemonName, (pokemon, caught) => new UserPokemonDetails
            {
                CatchDate = caught.CatchDate,
                Image = caught.IsShiny ? pokemon.ShinyImage : pokemon.Image,
                Name = caught.PokemonName,
                PokedexNum = pokemon.PokedexNum,
                IsShiny = caught.IsShiny
            }).ToList();
            ViewData["TotalPokemon"] = _userContext.Users.Find(userManager.GetUserId(User)).UniquePokemon;
            return View(caughtPokemon);
        }
    }
}

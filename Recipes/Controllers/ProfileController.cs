using Microsoft.AspNetCore.Mvc;
using Recipes.Models;
using Recipes.Core;

namespace Recipes.Controllers
{
    public class ProfileController : Controller
    {
        public readonly UserDbContext _context;
        public ProfileController(UserDbContext context) { 
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User user)
        {
            _context.Login(user.Email, user.Password);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            _context.Register(user);
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Logout()
        {
            _context.Logout();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Profile()
        {
            return View(_context.GetUser());
        }
        [HttpPost]
        [Route("Profile/AddFavorite")]
        public IActionResult AddFavorite(string id, string soort)
        {
            if (_context.GetUser() == null) return RedirectToAction("Login");
            var item = new Favorites() { recipeId = Convert.ToInt32(id), Soort = soort , UserId = _context.GetUser().UserId.ToString() };
            _context.AddFavoriteRecipe(item);
            return Ok();
        }

        [HttpPost]
        [Route("Profile/DelFavorite")]
        public IActionResult DelFavorite(string id, string soort)
        {
            var item = new Favorites() { recipeId = Convert.ToInt32(id), Soort = soort, UserId = _context.GetUser().UserId.ToString() };
            _context.DelFavoriteRecipe(item);
            return RedirectToAction("Profile");
        }
    }
}

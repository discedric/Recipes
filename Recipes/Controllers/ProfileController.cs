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
            _context.Login(user.Username);
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
    }
}

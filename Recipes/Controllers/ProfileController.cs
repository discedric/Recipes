using Microsoft.AspNetCore.Mvc;
using Recipes.Models;
using Recipes.Core;
using System.Text;

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
        [HttpGet]
        public ActionResult Settings()
        {
            UserSettings us = new()
            {
                Username = _context.LoggedIn.Username,
                Email = _context.LoggedIn.Email
            };
            return View(us);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Settings([FromForm]UserSettings us)
        {
            if(ModelState.IsValid)
            {
                var current = _context.LoggedIn;
                current.Username = us.Username;
                current.Email = us.Email;
                _context.updateUser(current);
                return RedirectToAction("Index", "Home");
            }
            return View(us);
        }
        [HttpGet]
        public IActionResult ChangePasswordPopup()
        {
            var model = new ChangePassword();
            return PartialView("ChangePasswordPopup",model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                var current = _context.LoggedIn;
                current.Password = model.NewPassword;
                _context.updateUser(current);
                return RedirectToAction("Settings");
            }
            return PartialView("_ChangePasswordPopup", model);
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

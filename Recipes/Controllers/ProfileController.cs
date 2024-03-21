using Microsoft.AspNetCore.Mvc;
using Recipes.Models;
using Recipes.Core;
using System.Text;
using System.Web;
using System.Security.Claims;

namespace Recipes.Controllers
{
    public class ProfileController : Controller
    {
        public readonly UserDbContext _context;
        string userId;

        public ProfileController(UserDbContext context)
        {
            _context = context;
            
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(CUser user)
        {
            userId = Request.Cookies["userId"];
            var duser = _context.Login(user.Email, user.Password);
            if (duser == null) return RedirectToAction("Login");
            Response.Cookies.Append("userId", duser._id.ToString());
            userId = duser._id.ToString();
            if (user.RememberMe)
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(399),
                    IsEssential = true
                };

                Response.Cookies.Append("email", user.Email, cookieOptions);
                Response.Cookies.Append("password", user.Password, cookieOptions);
            }
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(CUser user)
        {
            userId = Request.Cookies["userId"];
            _context.Register(user);
            Response.Cookies.Append("userId", user.UserId);
            userId = user.UserId;
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("userId");
            userId = null;
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Profile()
        {
            userId = Request.Cookies["userId"];
            var user = _context.GetUser(userId);
            user.Favorites = _context.GetFavorites(user);
            return View(user);
        }
        [HttpGet]
        public ActionResult Settings()
        {
            userId = Request.Cookies["userId"];
            UserSettings us = new()
            {
                Username = _context.GetUser(userId).Username,
                Email = _context.GetUser(userId).Email
            };
            return View(us);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Settings([FromForm]UserSettings us)
        {
            userId = Request.Cookies["userId"];
            if (ModelState.IsValid)
            {
                var current = _context.GetUser(userId);
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
            userId = Request.Cookies["userId"];
            if (ModelState.IsValid)
            {
                var current = _context.GetUser(userId);
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
            userId = Request.Cookies["userId"];
            if (_context.GetUser(userId) == null) return RedirectToAction("Login");
            var item = new Favorites() { recipeId = Convert.ToInt32(id), Soort = soort , UserId = userId };
            _context.AddFavoriteRecipe(item, _context.GetUser(userId));
            return Ok();
        }

        [HttpPost]
        [Route("Profile/DelFavorite")]
        public IActionResult DelFavorite(string id, string soort)
        {
            userId = Request.Cookies["userId"];
            var item = new Favorites() { recipeId = Convert.ToInt32(id), Soort = soort, UserId = userId };
            _context.DelFavoriteRecipe(item, _context.GetUser(userId));
            return RedirectToAction("Profile");
        }
    }
}

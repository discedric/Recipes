using Microsoft.AspNetCore.Mvc;

namespace Recipes.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}

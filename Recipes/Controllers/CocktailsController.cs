using Microsoft.AspNetCore.Mvc;

namespace Recipes.Controllers
{
    public class CocktailsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

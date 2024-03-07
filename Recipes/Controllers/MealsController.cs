using Microsoft.AspNetCore.Mvc;

namespace Recipes.Controllers
{
    public class MealsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

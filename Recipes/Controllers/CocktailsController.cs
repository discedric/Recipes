using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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


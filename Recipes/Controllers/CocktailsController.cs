using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Recipes.Models;

namespace Recipes.Controllers
{
    public class CocktailsController : Controller
    {
        private readonly ApiCocktails _apiDrinks = new ApiCocktails();
        [HttpGet]
        public IActionResult Index()
        {
            Categories.DrinkCategory mc = new() { drinks = _apiDrinks.GetRandomCocktails(new(), 20).Result, categories = _apiDrinks.GetCocktailCategories(new()).Result };
            return View(mc);
        }
    }
}


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
            return View(mc.drinks);
        }
        [HttpGet, HttpPost]
        public IActionResult CocktailRecipe(string id)
        {
            if (id == null)
                return RedirectToAction("Index");
            Cocktail? item;
            if (int.TryParse(id, out _))
                item = _apiDrinks.GetCocktailById(new(), id).Result;
            else
                item = _apiDrinks.GetCocktailByName(new(), id).Result;
            if (item == null)
                return RedirectToAction("Index");
            return View(item);
        }
        [HttpGet("/[controller]/MealCategory/{id}")]
        public IActionResult CocktailCategory([FromRoute] string id)
        {
            if (id == null)
                return RedirectToAction("Index");
            Categories.Catwithdrinks? cwm = _apiDrinks.GetCocktailsByCategory(new(), id).Result;
            if (cwm == null)
                return RedirectToAction("Index");
            return View(cwm);
        }
    }
}


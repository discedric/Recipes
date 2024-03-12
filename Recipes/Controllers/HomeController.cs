using Microsoft.AspNetCore.Mvc;
using Recipes.Models;
using System.Diagnostics;
using Recipes.Models;

namespace Recipes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApiMeals _apiMeals;
        private readonly ApiCocktails _apiCocktails;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _apiMeals = new ApiMeals();
            _apiCocktails = new ApiCocktails();
        }

        public IActionResult Index()
        {
            IList<MealItem> meals = _apiMeals.GetRandomMeals(new(),10).Result;
            IList<CocktailItem> drinks = _apiCocktails.GetRandomCocktails(new(),10).Result;
            Categories.mealDrinks mealDrinks = new()
            {
                meals = meals,
                drinks = drinks
            };
            return View(mealDrinks);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

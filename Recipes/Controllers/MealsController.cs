using Microsoft.AspNetCore.Mvc;
using Recipes.Models;

namespace Recipes.Controllers
{
    public class MealsController : Controller
    {
        private readonly ApiMeals _apiMeals = new ApiMeals();
        public IActionResult Index()
        {
            return View(_apiMeals.GetRandomMeals(new(),20).Result);
        }

        public IActionResult MealRecipe(string id)
        {
            if(id == null)
                return RedirectToAction("Index");
            Meal meal = _apiMeals.GetMealById(new(), id).Result;
            if(meal == null)
                return RedirectToAction("Index");
            return View(meal);
            
        }
    }
}

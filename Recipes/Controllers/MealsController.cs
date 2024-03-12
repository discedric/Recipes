using Microsoft.AspNetCore.Mvc;
using Recipes.Models;

namespace Recipes.Controllers
{
    public class MealsController : Controller
    {
        private readonly ApiMeals _apiMeals = new ApiMeals();
        [HttpGet]
        public IActionResult Index()
        {
            Categories.MealCategory mc = new() { meals = _apiMeals.GetRandomMeals(new(), 20).Result , categories = _apiMeals.GetMealCategories(new()).Result};
            return View(mc);
        }
        [HttpGet,HttpPost]
        public IActionResult MealRecipe(string id)
        {
            if(id == null)
                return RedirectToAction("Index");
            Meal? meal;
            if (int.TryParse(id, out _))
                meal = _apiMeals.GetMealById(new(), id).Result;
            else
                meal = _apiMeals.GetMealByName(new(), id).Result;
            if(meal == null)
                return RedirectToAction("Index");
            return View(meal);
        }
        [HttpGet("/[controller]/MealCategory/{id}")]
        public IActionResult MealCategory([FromRoute] string id)
        {
            if(id == null)
                return RedirectToAction("Index");
            Categories.Catwithmeals? cwm = _apiMeals.GetMealsByCategory(new(), id).Result;
            if(cwm == null)
                return RedirectToAction("Index");
            return View(cwm);
        }
    }
}

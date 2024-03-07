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
            MealCategory mc = new() { meals = _apiMeals.GetRandomMeals(new(), 20).Result , categories = _apiMeals.GetMealCategories(new()).Result};
            return View(mc);
        }
        [HttpGet]
        public IActionResult MealRecipe(string id)
        {
            if(id == null)
                return RedirectToAction("Index");
            Meal meal = _apiMeals.GetMealById(new(), id).Result;
            if(meal == null)
                return RedirectToAction("Index");
            return View(meal);
        }
        [HttpGet("/[controller]/MealCategory/{c}")]
        public IActionResult MealCategory([FromRoute] string c)
        {
            if(c == null)
                return RedirectToAction("Index");
            catwithmeals cwm = _apiMeals.GetMealsByCategory(new(), c).Result;
            if(cwm == null)
                return RedirectToAction("Index");
            return View(cwm);
        }
    }
}

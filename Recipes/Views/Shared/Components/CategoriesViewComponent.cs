using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Recipes.Models;

namespace Recipes.Views.Shared.Components
{
    public class CategoriesViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ApiMeals apiMeals = new();
            IList<Categories.Category> categories = await apiMeals.GetMealCategories(new());
            return View(categories);
        }
    }
}

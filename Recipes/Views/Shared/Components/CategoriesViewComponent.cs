using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Recipes.Models;

namespace Recipes.Views.Shared.Components
{
    public class CategoriesViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string type)
        {
            return View(type);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Recipes.Models;

namespace Recipes.Controllers
{
    public class CocktailsController : Controller
    {
        public IActionResult Index()
        {
            return View(LoadCocktails().Result);
        }

        public async Task<IList<Cocktail>> LoadCocktails()
        {
            ApiCocktails apiCocktails = new ApiCocktails();
            IList<Cocktail> cocktails = new List<Cocktail>();
            while (cocktails.Count < 20)
            {
                var cocktail = await apiCocktails._RandomCocktail();
                if (!cocktails.Contains(cocktail))
                    cocktails.Add(cocktail);
            }
            return cocktails;
        }
    }
}


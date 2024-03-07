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
            RandomCocktail randomCocktail = new RandomCocktail();
            IList<Cocktail> cocktails = new List<Cocktail>();
            while (cocktails.Count < 20)
            {
                var cocktail = await randomCocktail._RandomCocktail();
                if (!cocktails.Contains(cocktail))
                    cocktails.Add(cocktail);
            }
            return cocktails;
        }
    }
}


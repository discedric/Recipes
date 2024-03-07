using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Recipes.Controllers
{
    public class CocktailsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> RandomCocktail()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("https://www.thecocktaildb.com/api/json/v1/1/random.php");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<dynamic>(jsonString);
                    var cocktail = data.drinks[0];
                    Cocktail model = new Cocktail
                    {
                        IdDrink = cocktail.idDrink,
                        StrDrink = cocktail.strDrink,
                        StrDrinkAlternate = cocktail.strDrinkAlternate,
                        StrTags = cocktail.strTags,
                        StrVideo = cocktail.strVideo,
                        StrCategory = cocktail.strCategory,
                        StrIBA = cocktail.strIBA,
                        StrAlcoholic = cocktail.strAlcoholic,
                        StrGlass = cocktail.strGlass,
                        StrInstructions = cocktail.strInstructions,
                        StrDrinkThumb = cocktail.strDrinkThumb,
                        StrIngredients = new List<string>(),
                        StrMeasures = new List<string>(),
                        StrCreativeCommonsConfirmed = cocktail.strCreativeCommonsConfirmed,
                        DateModified = cocktail.dateModified
                    };

                    for (int i = 1; i <= 15; i++)
                    {
                        var ingredient = cocktail[$"strIngredient{i}"];
                        var measure = cocktail[$"strMeasure{i}"];
                        if (ingredient != null) model.StrIngredients.Add(ingredient.Value);
                        if (measure != null) model.StrMeasures.Add(measure.Value);
                    }

                    return View(model);
                }
                else
                {
                    return View("Error");
                }
            }
        }

    }
}


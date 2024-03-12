using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


namespace Recipes.Models;

public class ApiCocktails
{
    public async Task<CocktailItem?> GetRandomCocktail(HttpClient client)
    {
        string api = "https://www.thecocktaildb.com/api/json/v1/1/random.php";
        try
        {
            var json = await client.GetStringAsync(api);
            var response = JsonSerializer.Deserialize<CocktailItems>(json);
            CocktailItem? item;
            if (response == null)
                item = null;
            else
                item = response.cocktails[0];
            if (item == null)
                return null;
            return item;
        }
        catch
        {
            return null;
        }
    }

    public async Task<IList<CocktailItem>> GetRandomCocktails(HttpClient client, int count)
    {
        IList<CocktailItem> cocktails = new List<CocktailItem>();
        while (cocktails.Count < count)
        {
            var cocktail = await GetRandomCocktail(client);
            if (cocktail == null)
                continue;
            if (!cocktails.Contains(cocktail))
                cocktails.Add(cocktail);
        }
        return cocktails;
    }

    public async Task<Cocktail?> GetCocktailByName(HttpClient client, string name)
    {
        string api = $"https://www.thecocktaildb.com/api/json/v1/1/search.php?s={name}";
        try
        {
            var json = await client.GetStringAsync(api);
            var response = JsonSerializer.Deserialize<Cocktails>(json);
            if (response == null)
                return null;
            return Convert(response);
        }
        catch
        {
            return null;
        }
    }

    public async Task<Cocktail?> GetCocktailById(HttpClient client, string id)
    {
        string api = $"https://www.thecocktaildb.com/api/json/v1/1/lookup.php?i={id}";
        try
        {
            var json = await client.GetStringAsync(api);
            var response = JsonSerializer.Deserialize<Cocktails>(json);
            if (response == null)
                return null;
            return Convert(response);
        }
        catch
        {
            return null;
        }
    }

    public async Task<Catwithcocktails?> GetCocktailsByCategory(HttpClient client, string c)
    {
        string api = $"https://www.thecocktaildb.com/api/json/v1/1/filter.php?c={c}";
        try
        {
            var json = await client.GetStringAsync(api);
            var response = JsonSerializer.Deserialize<CocktailItems>(json);
            if (response == null)
                return null;
            Catwithcocktails cwc = new() { category = c, cocktails = response.cocktails };
            return cwc;
        }
        catch
        {
            return null;
        }
    }

    public async Task<IList<Category>?> GetCocktailCategories(HttpClient client)
    {
        string api = "https://www.thecocktaildb.com/api/json/v1/1/list.php?c=list";
        try
        {
            var json = await client.GetStringAsync(api);
            var response = JsonSerializer.Deserialize<CategoryList>(json);
            if (response == null)
                return null;
            return response.meals;
        }
        catch
        {
            return null;
        }
    }

    public Cocktail Convert(Cocktails cocktails)
    {
        var cocktailJson = cocktails.cocktails?[0];
        if (cocktailJson == null)
            return null;
        var cocktail = new Cocktail
        {
            idDrink = cocktailJson?.idDrink,
            strDrink = cocktailJson?.strDrink,
            strDrinkAlternate = cocktailJson?.strDrinkAlternate,
            strCategory = cocktailJson?.strCategory,
            strAlcoholic = cocktailJson?.strAlcoholic,
            strGlass = cocktailJson?.strGlass,
            strIBA = cocktailJson?.strIBA,
            strInstructions = cocktailJson?.strInstructions,
            strCocktailThumb = cocktailJson?.strCocktailThumb,
            strTags = cocktailJson?.strTags,
            strYoutube = cocktailJson?.strYoutube,
            strSource = cocktailJson?.strSource,
            strImageSource = cocktailJson?.strImageSource,
            strCreativeCommonsConfirmed = cocktailJson?.strCreativeCommonsConfirmed,
            dateModified = cocktailJson?.dateModified,
            Ingredients = new Dictionary<string, string>()
        };
        PopulateIngredients(cocktailJson, cocktail);

        return cocktail;
    }

    private void PopulateIngredients(CocktailJson cocktailJson, Cocktail cocktail)
    {
        for (int i = 1; i <= 20; i++)
        {
            string ingredient = (string)cocktailJson.GetType().GetProperty($"strIngredient{i}").GetValue(cocktailJson);
            string measure = (string)cocktailJson.GetType().GetProperty($"strMeasure{i}").GetValue(cocktailJson);

            if (!string.IsNullOrEmpty(ingredient) && !string.IsNullOrEmpty(measure))
            {
                if (!cocktail.Ingredients.ContainsKey(ingredient))
                    cocktail.Ingredients.Add(ingredient, measure);
                else
                    cocktail.Ingredients[ingredient] += $",{measure}";
            }
        }
    }
}


using System.Text.Json;

namespace Recipes.Models
{
    public class ApiMeals
    {
        public async Task<MealItem?> GetRandomMeal(HttpClient client)
        {
            string api = "https://www.themealdb.com/api/json/v1/1/random.php";
            try
            {
                var json = await client.GetStringAsync(api);
                var response = JsonSerializer.Deserialize<MealItems>(json);
                MealItem? item;
                if (response == null)
                    item = null;
                else
                    item = response.meals?[0];
                if (item == null)
                    return null;
                return item;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IList<MealItem>> GetRandomMeals(HttpClient client, int count)
        {
            IList<MealItem> meals = new List<MealItem>();
            while (meals.Count < count)
            {
                var cocktail = await GetRandomMeal(client);
                if (cocktail == null)
                    continue;
                if (!meals.Contains(cocktail))
                    meals.Add(cocktail);
            }
            return meals;
        }

        public async Task<MealItems?> GetMealByName(HttpClient client, string name)
        {
            string api = $"https://www.themealdb.com/api/json/v1/1/search.php?s={name}";
            try
            {
                var json = await client.GetStringAsync(api);
                var response = JsonSerializer.Deserialize<MealItems>(json);
                return response;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Meal?> GetMealById(HttpClient client, string id)
        {
            string api = $"https://www.themealdb.com/api/json/v1/1/lookup.php?i={id}";
            try
            {
                var json = await client.GetStringAsync(api);
                var response = JsonSerializer.Deserialize<Meals>(json);
                if (response == null)
                    return null;
                return Convert(response);
            }
            catch
            {
                return null;
            }
        }

        public async Task<Categories.Catwithmeals?> GetMealsByCategory(HttpClient client, string c)
        {
            string api = $"https://www.themealdb.com/api/json/v1/1/filter.php?c={c}";
            try
            {
                var json = await client.GetStringAsync(api);
                var response = JsonSerializer.Deserialize<MealItems>(json);
                if (response == null)
                    return null;
                Categories.Catwithmeals cwm = new() { category = c, meals = response.meals };
                return cwm;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IList<Categories.Category>?> GetMealCategories(HttpClient client)
        {
            string api = "https://www.themealdb.com/api/json/v1/1/list.php?c=list";
            try
            {
                var json = await client.GetStringAsync(api);
                var response = JsonSerializer.Deserialize<Categories.mCategoryList>(json);
                if (response == null)
                    return null;
                return response.meals;
            }
            catch
            {
                return null;
            }
        }

        private Meal Convert(Meals meals)
        {
            var mealJson = meals.meals?[0];
            if (mealJson == null)
                return null;
            var meal = new Meal
            {
                idMeal = mealJson?.idMeal,
                strMeal = mealJson?.strMeal,
                strDrinkAlternate = mealJson?.strDrinkAlternate,
                strCategory = mealJson?.strCategory,
                strArea = mealJson?.strArea,
                strInstructions = mealJson?.strInstructions,
                strMealThumb = mealJson?.strMealThumb,
                strTags = mealJson?.strTags,
                strYoutube = mealJson?.strYoutube,
                strSource = mealJson?.strSource,
                strImageSource = mealJson?.strImageSource,
                strCreativeCommonsConfirmed = mealJson?.strCreativeCommonsConfirmed,
                dateModified = mealJson?.dateModified,
                Ingredients = new Dictionary<string, string>()
            };
            PopulateIngredients(mealJson, meal);

            return meal;
        }

        private void PopulateIngredients(MealJson mealJson, Meal meal)
        {
            for (int i = 1; i <= 20; i++)
            {
                string ingredient = (string)mealJson.GetType().GetProperty($"strIngredient{i}").GetValue(mealJson);
                string measure = (string)mealJson.GetType().GetProperty($"strMeasure{i}").GetValue(mealJson);

                if (!string.IsNullOrEmpty(ingredient) && !string.IsNullOrEmpty(measure))
                {
                    if(!meal.Ingredients.ContainsKey(ingredient))
                        meal.Ingredients.Add(ingredient, measure);
                    else
                        meal.Ingredients[ingredient] += $",{measure}";
                }
            }
        }
    }
}

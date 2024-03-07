using System.Text.Json;

namespace Recipes.Models
{
    public class ApiMeals
    {
        public async Task<MealItem> GetRandomMeal(HttpClient client)
        {
            string api = "https://www.themealdb.com/api/json/v1/1/random.php";
            try
            {
                var json = await client.GetStringAsync(api);
                var response = JsonSerializer.Deserialize<MealItems>(json);
                MealItem item = response.meals[0];
                return item;
            }
            catch (Exception e)
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
                if (!meals.Contains(cocktail))
                    meals.Add(cocktail);
            }
            return meals;
        }

        public async Task<Meal> GetMealById(HttpClient client, string id)
        {
            string api = $"https://www.themealdb.com/api/json/v1/1/lookup.php?i={id}";
            try
            {
                var json = await client.GetStringAsync(api);
                var response = JsonSerializer.Deserialize<Meals>(json);
                return Convert(response);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<catwithmeals> GetMealsByCategory(HttpClient client, string c)
        {
            string api = $"https://www.themealdb.com/api/json/v1/1/filter.php?c={c}";
            try
            {
                var json = await client.GetStringAsync(api);
                var response = JsonSerializer.Deserialize<MealItems>(json);
                catwithmeals cwm = new() { category = c, meals = response.meals };
                return cwm;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<IList<Category>> GetMealCategories(HttpClient client)
        {
            string api = "https://www.themealdb.com/api/json/v1/1/list.php?c=list";
            try
            {
                var json = await client.GetStringAsync(api);
                var response = JsonSerializer.Deserialize<CategoryList>(json);
                return response.meals;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Meal Convert(Meals meals)
        {
            var mealJson = meals.meals[0];
            var meal = new Meal
            {
                idMeal = mealJson.idMeal,
                strMeal = mealJson.strMeal,
                strDrinkAlternate = mealJson.strDrinkAlternate,
                strCategory = mealJson.strCategory,
                strArea = mealJson.strArea,
                strInstructions = mealJson.strInstructions,
                strMealThumb = mealJson.strMealThumb,
                strTags = mealJson.strTags,
                strYoutube = mealJson.strYoutube,
                strSource = mealJson.strSource,
                strImageSource = mealJson.strImageSource,
                strCreativeCommonsConfirmed = mealJson.strCreativeCommonsConfirmed,
                dateModified = mealJson.dateModified,
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

                if (!string.IsNullOrWhiteSpace(ingredient))
                {
                    meal.Ingredients.Add(ingredient, measure);
                }
            }
        }
    }
}

namespace Recipes.Models
{
    public class Categories
    {
        public class mCategoryList
        {
            public Category[]? meals { get; set; }
        }
        public class cCategoryList
        {
            public Category[]? drinks { get; set; }
        }
        public class Category
        {
            public string? strCategory { get; set; }
        }
        public class Catwithmeals
        {
            public string? category { get; set; }
            public IList<MealItem>? meals { get; set; }
        }
        public class Catwithdrinks
        {
            public string? category { get; set; }
            public IList<CocktailItem>? drinks { get; set; }
        }
        public class MealCategory
        {
            public IList<MealItem>? meals { get; set; }
            public IList<Category>? categories { get; set; }
        }
        public class DrinkCategory
        {
            public IList<CocktailItem>? drinks { get; set; }
            public IList<Category>? categories { get; set; }
        }

        public class mealDrinks
        {
            public IList<MealItem>? meals { get; set; }
            public IList<CocktailItem>? drinks { get; set; }
        }
    }
}

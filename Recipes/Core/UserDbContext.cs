using Recipes.Models;
using System.Xml.Linq;

namespace Recipes.Core
{
    public class UserDbContext
    {
        public User? LoggedIn { get; set; } = new() { Email = "test@test", Username = "test" ,  Favorites= new List<Favorites>()};
        public UserDbContext()
        {
            // Connect to database
        }

        public void Register(User user)
        {
            // Add user to database
            LoggedIn = user;
        }
        public void Login(string username)
        {
            // Get user from database
            string email = "test";
            LoggedIn = new User() { Username = username, Email = email };
        }

        public void updateUser(User user)
        {
            // Update user in database
            LoggedIn = user;
        }

        public void AddFavoriteRecipe(Favorites recipe)
        {
            // Add recipe to user's favorites
            if(recipe == null)
                return;
            LoggedIn?.Favorites.Add(recipe);
        }

        public void DelFavoriteRecipe(Favorites recipe)
        {
            // Add recipe to user's favorites
            if (recipe == null)
                return;
            int index = LoggedIn?.Favorites
                .Select((favorite, i) => new { Favorite = favorite, Index = i })
                .FirstOrDefault(pair =>
                    pair.Favorite.RecipeId == recipe.RecipeId &&
                    pair.Favorite.Soort == recipe.Soort
                )?.Index ?? -1;
            if (index != -1)
                LoggedIn?.Favorites.RemoveAt(index);
        }

        public User? GetUser()
        {
            return LoggedIn;
        }

        public void Logout()
        {
            LoggedIn = null;
        }
    }
}

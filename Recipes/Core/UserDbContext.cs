using Recipes.Models;

namespace Recipes.Core
{
    public class UserDbContext
    {
        public User? LoggedIn { get; set; } = new() { Email = "test@test", Username = "test" };
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

using Recipes.Models;
using System.Xml.Linq;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Recipes.Core
{
    public class UserDbContext
    {
        public User? LoggedIn { get; set; }
        private readonly IMongoDatabase database;

        public UserDbContext()
        {
            const string connectionUri = "mongodb+srv://matheoD:Fackschool@cluster0.zek7qbd.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";
            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            database = client.GetDatabase("recipes");
            // Connect to database
            try
            {
                database.RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void Register(User user)
        {
            // Add user to database
            var collection = database.GetCollection<DUser>("users");
            DUser dUser = new DUser() { username = user.Username, email = user.Email, password = user.Password };
            collection.InsertOne(dUser);
            Console.WriteLine("User added to database");
            LoggedIn = user;
        }
        public void Login(string mail, string password)
        {
            // Get user from database
            var collection = database.GetCollection<DUser>("users");
            var filter = Builders<DUser>.Filter.Eq(u => u.email, mail) & Builders<DUser>.Filter.Eq(u => u.password, password);
            var user = collection.Find(filter).FirstOrDefault();
            if (user == null)
            {
                Console.WriteLine("Invalid email or password");
                return;
            }
            Console.WriteLine("User found in database");

            LoggedIn = new User() { UserId = user._id.ToString(), Username = user.username, Email = user.email };
            
            LoggedIn.Favorites = GetFavorites();
        }

        public void updateUser(User user)
        {
            // Update user in database
            LoggedIn = user;
        }

        public IList<Favorites> GetFavorites()
        {
            var fcollection = database.GetCollection<Favorites>("favourits");
            var ffilter = Builders<Favorites>.Filter.Eq(u => u.UserId, LoggedIn.UserId);
            var favorites = fcollection.Find(ffilter).ToList();
            return favorites;
        }

        public void AddFavoriteRecipe(Favorites recipe)
        {
            // Add recipe to user's favorites
            if(recipe == null)
                return;
            var collection = database.GetCollection<Favorites>("favourits");
            Favorites favorite = new Favorites() { _id = new ObjectId(), UserId = LoggedIn.UserId, Soort = recipe.Soort, recipeId = recipe.recipeId };
            collection.InsertOne(favorite);
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
                    pair.Favorite.recipeId == recipe.recipeId &&
                    pair.Favorite.Soort == recipe.Soort
                )?.Index ?? -1;
            if (index != -1)
            {
                LoggedIn?.Favorites.RemoveAt(index);
                var collection = database.GetCollection<Favorites>("favourits");
                var filter =    Builders<Favorites>.Filter.Eq(f => f.UserId, LoggedIn.UserId) & 
                                Builders<Favorites>.Filter.Eq(f => f.recipeId, recipe.recipeId) & 
                                Builders<Favorites>.Filter.Eq(f => f.Soort, recipe.Soort);
                collection.DeleteOne(filter);
                collection.Database.GetCollection<Favorites>("favourits").DeleteOne(filter);
            }
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

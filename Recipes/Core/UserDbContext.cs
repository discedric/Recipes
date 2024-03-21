using Recipes.Models;
using System.Xml.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text;
using System.Text.Json;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Amazon.Runtime.Documents;
using System.Net;
using System.Diagnostics.Eventing.Reader;
using Microsoft.AspNetCore.Mvc;


namespace Recipes.Core
{
    public class UserDbContext
    {
        private readonly IMongoDatabase database;
        public UserDbContext()
        {

            var json = JsonDocument.Parse(File.ReadAllText("settings.json"));
            string connectionUri = json.RootElement.GetProperty("ConnectionString").GetString();

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

        public void Register(CUser user)
        {
            // Add user to database
            var collection = database.GetCollection<DUser>("users");
            DUser dUser = new DUser() { username = user.Username, email = user.Email, password = Encoding.ASCII.GetBytes(user.Password) };
            collection.InsertOne(dUser);
            var filter = Builders<DUser>.Filter.Eq(u => u.email, user.Email) & Builders<DUser>.Filter.Eq(u => u.password, Encoding.ASCII.GetBytes(user.Password));
            var duser = collection.Find(filter).FirstOrDefault();
            Console.WriteLine("User added to database");
            //LoggedIn = user;
        }
        public DUser Login(string mail, string pass)
        {
            // Get user from database
            var password = Encoding.ASCII.GetBytes(pass);
            var collection = database.GetCollection<DUser>("users");
            var filter = Builders<DUser>.Filter.Eq(u => u.email, mail) & Builders<DUser>.Filter.Eq(u => u.password, password);
            var user = collection.Find(filter).FirstOrDefault();
            if (user == null)
            {
                Console.WriteLine("Invalid email or password");
                return null;
            }
            Console.WriteLine("User found in database");
            return user;
        }

        public DUser updateUser(CUser user)
        {
            var collection = database.GetCollection<DUser>("users");
            ObjectId.TryParse(user.UserId, out var id);
            var filter = Builders<DUser>.Filter.Eq(u => u._id, id);
            var update = Builders<DUser>.Update
                .Set(u => u._id, id)
                .Set(u => u.username, user.Username)
                .Set(u => u.email, user.Email)
                .Set(u => u.password, Encoding.ASCII.GetBytes(user.Password));
            var result = collection.UpdateOne(filter, update);
            if (result.ModifiedCount > 0)
            {
                return new() { _id = ObjectId.Parse(user.UserId), username = user.Username, email = user.Email, password = Encoding.ASCII.GetBytes(user.Password)};
            }
            else
            {
                return null;
            }
        }

        public IList<Favorites> GetFavorites(CUser user)
        {
            var fcollection = database.GetCollection<Favorites>("favourits");
            var ffilter = Builders<Favorites>.Filter.Eq(u => u.UserId, user.UserId);
            var favorites = fcollection.Find(ffilter).ToList();
            return favorites;
        }

        public void AddFavoriteRecipe(Favorites recipe, CUser user)
        {
            // Add recipe to user's favorites
            if(recipe == null)
                return;
            var collection = database.GetCollection<Favorites>("favourits");
            Favorites favorite = new Favorites() { _id = new ObjectId(), UserId = user.UserId, Soort = recipe.Soort, recipeId = recipe.recipeId };
            collection.InsertOne(favorite);
        }

        public void DelFavoriteRecipe(Favorites recipe, CUser user)
        {
            // Add recipe to user's favorites
            if (recipe == null)
                return;
            if (true)
            {
                var collection = database.GetCollection<Favorites>("favourits");
                var filter =    Builders<Favorites>.Filter.Eq(f => f.UserId, user.UserId) & 
                                Builders<Favorites>.Filter.Eq(f => f.recipeId, recipe.recipeId) & 
                                Builders<Favorites>.Filter.Eq(f => f.Soort, recipe.Soort);
                collection.DeleteOne(filter);
                collection.Database.GetCollection<Favorites>("favourits").DeleteOne(filter);
            }
        }

        public CUser? GetUser(String UserId)
        {
            if(UserId == null)
                return null;
            var collection = database.GetCollection<DUser>("users");
            var filter = Builders<DUser>.Filter.Eq(u => u._id, ObjectId.Parse(UserId));
            var user = collection.Find(filter).FirstOrDefault();
            if (user == null)
            {
                Console.WriteLine("User not found in database");
                return null;
            }
            var cUser = new CUser() { UserId = user._id.ToString(), Username = user.username, Email = user.email, Password = Encoding.ASCII.GetString(user.password) };
            return cUser;
        }
    }
}

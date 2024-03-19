using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Recipes.Models
{
    public class User
    {
        public string UserId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
        public IList<Favorites>? Favorites { get; set; }
    }

    public class DUser
    {
        public ObjectId _id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }

    public class Favorites
    {
        public ObjectId _id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Soort { get; set; }
        public int recipeId { get; set; }
    }
}

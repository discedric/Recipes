namespace Recipes.Models
{
    public class User
    {
        public int UserId { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public string Password { get; set; }
        public IList<Favorites>? Favorites { get; set; }
    }

    public class Favorites
    {
        public required int UserId { get; set; }
        public required string Soort { get; set; }
        public required int RecipeId { get; set; }
    }
}

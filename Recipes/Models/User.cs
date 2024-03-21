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
        public Byte[] password { get; set; }
    }

    public class UserSettings
    {
        [Required(ErrorMessage = "Please enter your username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
    }

    public class ChangePassword
    {
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public bool IsNewPasswordProvided => !string.IsNullOrEmpty(NewPassword);
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

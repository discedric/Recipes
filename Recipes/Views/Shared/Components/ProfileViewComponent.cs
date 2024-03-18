using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Recipes.Core;
using Recipes.Models;

namespace Recipes.Views.Shared.Components
{
    public class ProfileViewComponent : ViewComponent
    {
        private UserDbContext userDb { get; set; }
        public ProfileViewComponent(UserDbContext userDb)
        {
            this.userDb = userDb;
        }
        public async Task<IViewComponentResult> InvokeAsync(string type)
        {
            ViewBag.User = userDb.LoggedIn;
            return View(type);
        }
    }
}

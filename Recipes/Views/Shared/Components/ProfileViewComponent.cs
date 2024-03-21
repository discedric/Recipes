using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Recipes.Core;
using System.Web;
using Recipes.Models;

namespace Recipes.Views.Shared.Components
{
    public class ProfileViewComponent : ViewComponent
    {
        private UserDbContext _context;
        string userId;
        public ProfileViewComponent(UserDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string type)
        {
            userId = Request.Cookies["userId"];
            var user = _context.GetUser(userId);
            if(user != null)
                ViewBag.User = user;
            return View(type);
        }
    }
}

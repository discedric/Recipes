using Recipes.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<UserDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    using var scope = app.Services.CreateScope();
    var userDbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
}
app.Use(async (context, next) =>
{
    var email = context.Request.Cookies["email"];
    var password = context.Request.Cookies["password"];

    if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
    {
        using var scope = app.Services.CreateScope();
        var userDbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        userDbContext.Login(email, password);
    }

    await next.Invoke();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

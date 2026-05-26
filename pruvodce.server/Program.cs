using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("Sqlite")
        ?? "Data Source=pruvodce.db"
    ));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 1;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/login";
    options.AccessDeniedPath = "/login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AllowAnonymousToPage("/Login");

    options.Conventions.AuthorizeFolder("/Students", "StudentPolicy");

    options.Conventions.AuthorizeFolder("/");
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("StudentPolicy", policy =>
        policy.RequireRole("Student", "Admin"));
});

builder.Services.AddScoped<MapDataService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedIdentityData(services);
}

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

/*
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower();
    
    if (path == "/admin" || path == "/admin/")
    {
        if (!context.User?.Identity?.IsAuthenticated == true)
        {
            context.Response.Redirect("/login");
            return;
        }
    }
    
    await next();
});*/

app.MapControllers();
app.MapRazorPages();

app.Run();

async Task SeedIdentityData(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string[] roleNames = { "Admin", "Student" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    var adminUser = await userManager.FindByNameAsync("admin");
    if (adminUser == null)
    {
        adminUser = new IdentityUser
        {
            UserName = "admin",
            Email = "admin@pruvodce.cz",
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(adminUser, "admin123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }

    var studentUser = await userManager.FindByNameAsync("student");
    if (studentUser == null)
    {
        studentUser = new IdentityUser
        {
            UserName = "student",
            Email = "student@pruvodce.cz",
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(studentUser, "student123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(studentUser, "Student");
        }
    }
}
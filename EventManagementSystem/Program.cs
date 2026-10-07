using EventManagementSystem.Data;
using EventManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// -----------------
// Configure Database
// -----------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// -----------------
// Configure Identity
// -----------------
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // optional for testing
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// -----------------
// Add MVC / Razor
// -----------------
builder.Services.AddControllersWithViews();

// -----------------
// Build app
// -----------------
var app = builder.Build();

// -----------------
// Seed roles and assign first user as Admin
// -----------------
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // 1. Create roles if they don't exist
    if (!roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
    {
        roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
    }

    if (!roleManager.RoleExistsAsync("User").GetAwaiter().GetResult())
    {
        roleManager.CreateAsync(new IdentityRole("User")).GetAwaiter().GetResult();
    }

    // 2. Assign first registered user to Admin role
    var firstUser = userManager.Users.FirstOrDefault();
    if (firstUser != null && !userManager.IsInRoleAsync(firstUser, "Admin").GetAwaiter().GetResult())
    {
        userManager.AddToRoleAsync(firstUser, "Admin").GetAwaiter().GetResult();
    }
}

// -----------------
// Configure middleware pipeline
// -----------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // must be before authorization
app.UseAuthorization();

// -----------------
// Map routes
// -----------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Identity

// -----------------
// Run app
// -----------------
app.Run();

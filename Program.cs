using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vigilante.Data;
using Vigilante.Models;
using Vigilante.Services;
using Vigilante.Services.Factories;
using Vigilante.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = DataUtility.GetConnectionString(builder.Configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<VGUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddClaimsPrincipalFactory<VGUserClaimsPrincipleFactory>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

// ----------------------------------------------------------------------
// ADD THIS SECTION TO CONFIGURE THE APPLICATION COOKIE
// This controls how long the user stays logged in and if the cookie is persistent
builder.Services.ConfigureApplicationCookie(options =>
{
    // Configure your cookie here:

    // Cookie settings
    options.Cookie.HttpOnly = true; // Prevents client-side script access to the cookie
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20); // DEFAULT non-persistent timeout (e.g., 20 minutes)

    options.LoginPath = "/Identity/Account/Login"; // Path to your login page
    options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // Path for access denied
    options.SlidingExpiration = true; // Renews cookie expiration time on activity

    // To force non-persistent login (user is logged out when browser is closed):
    // options.Cookie.IsPersistent = false; // This is typically the default unless "Remember Me" is checked.
    // You usually let the "Remember Me" checkbox on the login page control this.

    // If you want to force persistent login (e.g., for 30 days) regardless of "Remember Me":
    // options.ExpireTimeSpan = TimeSpan.FromDays(30);
    // options.Cookie.IsPersistent = true; // This would cause "always logged in" behavior if set.

    // To explicitly handle "Remember Me" functionality, you'd typically remove
    // explicit `options.Cookie.IsPersistent = true;` here, and let the `isPersistent` parameter
    // in `SignInManager.PasswordSignInAsync` control it.
});
// ---------------------------------------------------------------------------

//dependency injection for custom services
builder.Services.AddScoped<IVGRolesService, VGRolesService>();
    builder.Services.AddScoped<IVGCompanyInfoService, VGCompanyInfoService>();
    builder.Services.AddScoped<IVGProjectService, VGProjectService>();
    builder.Services.AddScoped<IVGTicketService, VGTicketService>();
    builder.Services.AddScoped<IVGHistoryService, VGHistoryService>();
    builder.Services.AddScoped<IVGNotificationService, VGNotificationService>();
    builder.Services.AddScoped<IVGInviteService, VGInviteService>();
    builder.Services.AddScoped<IVGFileService, VGFileService>();
builder.Services.AddScoped<IVGLookUpService, VGLookUpService>();
    builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

//default  
builder.Services.AddControllersWithViews();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();

        //Pass the 'app' variable directly
        await DataUtility.ManageDataAsync(app);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

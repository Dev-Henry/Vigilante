using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Vigilante.Data;
using Vigilante.Models;
using Vigilante.Services;
using Vigilante.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = DataUtility.GetConnectionString(builder.Configuration);


// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(DataUtility.GetConnectionString(builder.Configuration)));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<VGUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

//dependency injection for custom services
builder.Services.AddScoped<IVGRolesService, VGRolesService>();
builder.Services.AddScoped<IVGCompanyInfoService, VGCompanyInfoService>();
builder.Services.AddScoped<IVGProjectService, VGProjectService>();
builder.Services.AddScoped<IVGTicketService, VGTicketService>();
//builder.Services.AddScoped<IVGTicketHistoryService, VGHistoryService>();
//builder.Services.AddScoped<IVGNotificationService, VGNotificationService>();
//builder.Services.AddScoped<IVGInviteService, VGInviteService>();
//builder.Services.AddScoped<IVGFileService, VGFileService>();

//default  
builder.Services.AddControllersWithViews();



var app = builder.Build();

//DI for seeded data
//var scope = app.Services.CreateScope();
//await DataUtility.ManageDataAsync(scope.ServiceProvider);

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

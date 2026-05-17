using MudBlazor.Services;
using eTourismWebApp.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using eTourismWebApp.Data;
using eTourismWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("eTourismWebAppContextConnection")
                       ?? throw new InvalidOperationException("Connection string 'eTourismWebAppContextConnection' not found.");

builder.Services.AddDbContext<eTourismWebAppContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<eTourismWebAppContext>();

builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    });

builder.Services.AddRazorPages();
builder.Services.AddAuthorization();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IChartService, ChartService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddLogging();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// ✔ Varianta corectă pentru .NET 8
app.UseStatusCodePagesWithReExecute("/not-found");

app.UseHttpsRedirection();

// ✔ În .NET 8 se folosește UseStaticFiles(), nu MapStaticAssets()
app.UseStaticFiles();

app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

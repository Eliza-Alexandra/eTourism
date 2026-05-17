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
        options.SignIn.RequireConfirmedAccount = false; // în dev poți pune false
        
        // Configure lockout settings
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); // Lockout duration
        options.Lockout.MaxFailedAccessAttempts = 5; // Maximum failed attempts before lockout
        options.Lockout.AllowedForNewUsers = true; // Enable lockout for new users
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<eTourismWebAppContext>();

builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    });


builder.Services.AddRazorPages(); // <- NECESAR pentru /Identity/Account/*
builder.Services.AddAuthorization();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

// Dashboard and Chart Services
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IChartService, ChartService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddLogging();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.MapStaticAssets();

app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages(); // <- expune /Identity/Account/Login, Register, Logout

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
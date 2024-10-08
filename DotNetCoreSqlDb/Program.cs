using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DotNetCoreSqlDb.Data;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;
using SendGrid;
using SendGrid.Helpers.Mail;
using DotNetCoreSqlDb.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using DotNetCoreSqlDb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration.AddEnvironmentVariables();


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<MyDatabaseContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Lösenordsinställningar
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;


    
});

builder.Services.ConfigureApplicationCookie(options =>
{
   
    options.ExpireTimeSpan = TimeSpan.FromHours(8);

    options.AccessDeniedPath = "/Account/AccessDenied";
    
    
    options.SlidingExpiration = true;
});

builder.Services.AddHttpClient();

builder.Services.AddSingleton<ISendGridClient>(x => 
{
    var configuration = x.GetRequiredService<IConfiguration>();
    var sendGridApiKey = configuration["SendGridApiKey"];
    
    return new SendGridClient(new SendGridClientOptions
    {
        ApiKey = sendGridApiKey
    });
});

builder.Services.AddTransient<IEmailService, EmailService>();

// Add database context and cache
builder.Services.AddDbContext<MyDatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDbConnection")));
builder.Services.AddStackExchangeRedisCache(options =>
{
options.Configuration = builder.Configuration["AZURE_REDIS_CONNECTIONSTRING"];
options.InstanceName = "SampleInstance";
});


builder.Services.AddControllersWithViews();


builder.Logging.AddAzureWebAppDiagnostics();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
  
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// IMPORTANT ADDITIONS
app.UseAuthentication();
app.UseAuthorization();

using var serviceScope = app.Services.CreateScope();
await EnsureRolesCreated(serviceScope.ServiceProvider);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Sites}/{action=Index}/{id?}");

app.Run();

static async Task EnsureRolesCreated(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new List<string> { "Admin", "User", "Editor", "PrinterAdmin", "KassaAdmin"}; 

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

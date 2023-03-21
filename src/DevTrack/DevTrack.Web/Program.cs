using Autofac;
using Autofac.Extensions.DependencyInjection;
using DemoWebApp.Infrastructure.Services;
using DevTrack.Infrastructure;
using DevTrack.Infrastructure.DbContexts;
using DevTrack.Infrastructure.Entities;
using DevTrack.Infrastructure.Services;
using DevTrack.Web;
using DevTrack.Web.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Reflection;

try
{
    var builder = WebApplication.CreateBuilder(args);
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var assemblyName = Assembly.GetExecutingAssembly().FullName;

    //Serilog Configure
    builder.Host.UseSerilog((ctx, lc) => lc
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(builder.Configuration));

    //Autofac Configure
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new WebModule());
        containerBuilder.RegisterModule(new InfrastructureModule(connectionString, assemblyName));
    });

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlServer(connectionString, m => m.MigrationsAssembly(assemblyName)));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    //AutoMapper Configure
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    //Identity Configurations
    builder.Services
   .AddIdentity<ApplicationUser, ApplicationRole>()
   .AddEntityFrameworkStores<ApplicationDbContext>()
   .AddUserManager<ApplicationUserManager>()
   .AddRoleManager<ApplicationRoleManager>()
   .AddSignInManager<ApplicationSignInManager>()
   .AddDefaultTokenProviders();

    builder.Services.AddAuthentication()
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, option =>
        {
            option.LogoutPath = new PathString("/Account/Login");
            option.LogoutPath = new PathString("/Account/Logout");
            option.AccessDeniedPath = new PathString("/Account/Login");
            option.Cookie.Name = "DevTrackPortal.Identity";
            option.SlidingExpiration = true;
            option.ExpireTimeSpan = TimeSpan.FromHours(1);
        });

    builder.Services.Configure<IdentityOptions>(options =>
    {
        // Password settings.
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 0;

        // Lockout settings.
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        // User settings.
        options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true;
    });

    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    var siteSettings = builder.Configuration.GetSection("SiteSettings");

    builder.Services.Configure<SiteSettings>(siteSettings);

    builder.Services.AddControllersWithViews();

    var useUrl = siteSettings.GetValue<bool>("UseUrlSettings");

    if (useUrl)
        builder.WebHost.UseUrls("http://*:80");

    var app = builder.Build();

    Log.Information("Application Starting up...");

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

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseSession();

    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Project}/{action=Index}/{id?}");
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");    

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal("Application Start up Failed!", ex.Message);
}
finally
{
    Log.CloseAndFlush();
}
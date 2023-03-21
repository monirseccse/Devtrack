using Autofac;
using Autofac.Extensions.DependencyInjection;
using DevTrack.Api;
using DevTrack.Infrastructure;
using DevTrack.Infrastructure.DbContexts;
using DevTrack.Infrastructure.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Reflection;
using System.Text;

try
{
    var builder = WebApplication.CreateBuilder(args);

    //GetConnectionString
    var connectionstring = builder.Configuration.GetConnectionString("DefaultConnection");

    //getAsseblyName
    var assembly = Assembly.GetExecutingAssembly().FullName;

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //Serilog Configuration
    Log.Logger = new LoggerConfiguration()
    .WriteTo.MSSqlServer(connectionString: connectionstring,
          sinkOptions: new MSSqlServerSinkOptions { TableName = "LogsApi", AutoCreateSqlTable = true })
    .WriteTo.File("Logs/DevTrackLog-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

    //Database Configuration
    builder.Services.AddDbContext<ApplicationDbContext>(option =>
    {
        option.UseSqlServer(connectionstring, e => e.MigrationsAssembly(assembly));
    });

    //Add Custom identity
    builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

    //Set Identity Option
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

    //setup JWT Barer Token int Authentication Configuration
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWT:Key"))),
            ValidateLifetime = true,
            ValidateAudience = false,
            ValidateIssuer = false,
            ClockSkew = TimeSpan.Zero
        };
        options.SaveToken = true;
    });

    //set Authorization
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("ClientrequireMentPolicy", options =>
        {
            options.AuthenticationSchemes.Clear();
            options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            options.RequireAuthenticatedUser();
            options.RequireClaim("Client");

        });
    });

    //Autofac Configure
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(option =>
    {
        option.RegisterModule(new ApiModule());
        option.RegisterModule(new InfrastructureModule(connectionstring, assembly));
    });

    //Configure Automapper
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    var app = builder.Build();

    Log.Write(LogEventLevel.Information, "Hi Application Start");

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Log.Write(LogEventLevel.Fatal, ex.Message);
}
finally
{
    Log.CloseAndFlush();
}
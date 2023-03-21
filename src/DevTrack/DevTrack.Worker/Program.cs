using Autofac;
using Autofac.Extensions.DependencyInjection;
using DevTrack.Worker;
using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Serilog;
using DevTrack.Infrastructure;
using DevTrack.Infrastructure.DbContexts;
using DevTrack.Infrastructure.BusinessObjects;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", false)
                .AddEnvironmentVariables()
                .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");

var migrationAssemblyName = typeof(Worker).Assembly.FullName;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    Log.Information("Application Starting up");

    IHost host = Host.CreateDefaultBuilder(args)
        .UseWindowsService()
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .UseSerilog()
        .ConfigureContainer<ContainerBuilder>(builder =>
        {
            builder.RegisterModule(new WorkerModule());
            builder.RegisterModule(new InfrastructureModule(connectionString, migrationAssemblyName));
        })
        .ConfigureServices((builder, services) =>
        {
            services.AddHostedService<Worker>();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, m => m.MigrationsAssembly(migrationAssemblyName)));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<Smtp>(builder.Configuration.GetSection("Smtp"));
        })
        .Build();

    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}

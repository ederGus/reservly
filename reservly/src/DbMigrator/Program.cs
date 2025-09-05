using DbMigrator.Configuration;
using DbMigrator.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddEnvironmentVariables()
              .AddJsonFile("appsettings.json", optional: true)
              .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true);
    })
    .ConfigureServices((context, services) =>
    {
        // Config
        services.Configure<MongoDbSettings>(context.Configuration.GetSection("MongoDB"));

        // Services
        services.AddSingleton<IMongoDbService, MongoDbService>();
        services.AddSingleton<IMigrationService, MigrationService>();
        services.AddSingleton<IMigrationDiscovery, MigrationDiscovery>();

        // Logging
        services.AddLogging(builder => builder.AddConsole());
    })
    .ConfigureLogging(logging =>
     {
         logging.ClearProviders();
         logging.AddConsole();
         logging.SetMinimumLevel(LogLevel.Information);
     })
    .Build();


try
{
    // MongoDB serializers config
    MongoDbConfiguration.Configure();

    var migrationService = host.Services.GetRequiredService<IMigrationService>();
    var logger = host.Services.GetRequiredService<ILogger<Program>>();

    logger.LogInformation("Starting database migrations...");

    using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(30));
    await migrationService.RunMigrationsAsync(cts.Token);

    logger.LogInformation("Database migrations completed successfully.");
}
catch (OperationCanceledException)
{
    Console.WriteLine("Migration process was cancelled.");
    Environment.Exit(1);
}
catch (Exception ex)
{
    Console.WriteLine($"Migration failed: {ex.Message}");
    Environment.Exit(1);
}
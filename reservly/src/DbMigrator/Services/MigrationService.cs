namespace DbMigrator.Services
{
    using DbMigrator.Models;
    using Microsoft.Extensions.Logging;
    using MongoDB.Driver;
    using System.Threading;
    using System.Threading.Tasks;

    public class MigrationService : IMigrationService
    {
        private readonly IMongoDbService _mongoDbService;
        private readonly IMigrationDiscovery _migrationDiscovery;
        private readonly ILogger<MigrationService> _logger;

        public MigrationService(IMongoDbService mongoDbService, IMigrationDiscovery migrationDiscovery, ILogger<MigrationService> logger)
        {
            _mongoDbService = mongoDbService;
            _migrationDiscovery = migrationDiscovery;
            _logger = logger;
        }

        public async Task RunMigrationsAsync(CancellationToken cancellationToken = default)
        {
            if (!await _mongoDbService.TestConnectionAsync(cancellationToken))
            {
                throw new InvalidOperationException("Cannot connect to MongoDB");
            }

            var database = _mongoDbService.GetDatabase();
            var migrations = _migrationDiscovery.DiscoverMigrations().ToList();
            var appliedMigrations = await GetAppliedMigrationsAsync(database, cancellationToken);

            foreach (var migration in migrations)
            {
                if (appliedMigrations.Contains(migration.Name))
                {
                    _logger.LogInformation("[SKIP] {MigrationName}", migration.Name);
                    continue;
                }

                _logger.LogInformation("[APPLY] {MigrationName}", migration.Name);

                await migration.UpAsync(database, cancellationToken);
                await migration.VerifyAsync(database, cancellationToken);

                _logger.LogInformation("[DONE] {MigrationName}", migration.Name);
            }
        }

        private static async Task<HashSet<string>> GetAppliedMigrationsAsync(IMongoDatabase database, CancellationToken cancellationToken)
        {
            var collection = database.GetCollection<MigrationRecord>("_migrations");

            var applied = await collection
                .Find(_ => true)
                .ToListAsync(cancellationToken);

            return applied.Select(x => x.Name).ToHashSet();
        }
    }
}

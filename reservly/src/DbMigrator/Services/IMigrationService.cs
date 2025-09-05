namespace DbMigrator.Services
{
    public interface IMigrationService
    {
        Task RunMigrationsAsync(CancellationToken cancellationToken = default);
    }
}

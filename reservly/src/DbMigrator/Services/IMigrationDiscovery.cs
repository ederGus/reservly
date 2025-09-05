namespace DbMigrator.Services
{
    using DbMigrator.Migrations;

    public interface IMigrationDiscovery
    {
        IEnumerable<IMigration> DiscoverMigrations();
    }
}

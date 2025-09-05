namespace DbMigrator.Services
{
    using DbMigrator.Migrations;

    public class MigrationDiscovery : IMigrationDiscovery
    {
        public IEnumerable<IMigration> DiscoverMigrations()
        {
            return new IMigration[]
            {
                new Migration_0001_InitializeDatabase(),
            }
            .OrderBy(m => m.Name);
        }
    }
}

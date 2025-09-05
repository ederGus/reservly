namespace DbMigrator.Migrations
{
    using DbMigrator.Models;
    using MongoDB.Driver;

    public class Migration_0001_InitializeDatabase : IMigration
    {
        public string Name => "0001_InitializeDatabase";

        public async Task UpAsync(IMongoDatabase db, CancellationToken ct)
        {
            // Crea colecciones si no existen
            var collections = (await db.ListCollectionNamesAsync(cancellationToken: ct)).ToList(cancellationToken: ct);

            async Task EnsureCollection(string name)
            {
                if (!collections.Contains(name))
                {
                    await db.CreateCollectionAsync(name, cancellationToken: ct);
                }
            }

            await EnsureCollection("events");
            await EnsureCollection("reservations");
            await EnsureCollection("payments");
            await EnsureCollection("users");
            await EnsureCollection("_migrations");

            await CreateReservationIndexes(db, ct);

            await MarkMigrationAsApplied(db, ct);
        }

        private static async Task CreateReservationIndexes(IMongoDatabase db, CancellationToken ct)
        {
            var reservations = db.GetCollection<ReservationDocument>("reservations");
            var idxBuilder = Builders<ReservationDocument>.IndexKeys;

            var models = new List<CreateIndexModel<ReservationDocument>>
            {
                new(idxBuilder.Ascending(x => x.UserId)),
                new(idxBuilder.Ascending(x => x.EventId).Ascending(x => x.SessionId).Ascending(x => x.Status)),
                new(idxBuilder.Ascending(x => x.HoldExpiresAt)),
                new(idxBuilder.Ascending(x => x.QrToken).Ascending(x => x.Status),
                    new CreateIndexOptions { Unique = true, Sparse = true })
            };

            await reservations.Indexes.CreateManyAsync(models, ct);
        }

        private async Task MarkMigrationAsApplied(IMongoDatabase db, CancellationToken ct)
        {
            var migrations = db.GetCollection<MigrationRecord>("_migrations");
            var existing = await migrations.Find(x => string.Equals(x.Name, Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync(ct);

            if (existing == null)
            {
                await migrations.InsertOneAsync(new MigrationRecord
                {
                    Name = Name,
                    AppliedAtUtc = DateTime.UtcNow
                }, cancellationToken: ct);
            }
        }
    }
}

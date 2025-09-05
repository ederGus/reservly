namespace DbMigrator.Migrations
{
    using MongoDB.Driver;

    public interface IMigration
    {
        /// <summary>
        /// Unique name of the migration (e.g., "0001_CreateBase").
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Executes the migration. Must be idempotent.
        /// </summary>
        /// <param name="db">MongoDB database instance</param>
        /// <param name="cancellationToken">Token to cancel the operation if needed</param>
        Task UpAsync(IMongoDatabase db, CancellationToken cancellationToken = default);

        /// <summary>
        /// Optional post-migration validation.
        /// </summary>
        /// <param name="db">MongoDB database instance</param>
        /// <param name="cancellationToken">Token to cancel the operation if needed</param>
        Task VerifyAsync(IMongoDatabase db, CancellationToken cancellationToken = default)
            => Task.CompletedTask;
    }
}

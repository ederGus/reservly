namespace DbMigrator.Services
{
    using MongoDB.Driver;

    public interface IMongoDbService
    {
        IMongoDatabase GetDatabase();

        Task<bool> TestConnectionAsync(CancellationToken cancellationToken = default);
    }
}

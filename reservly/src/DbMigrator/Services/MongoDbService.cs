namespace DbMigrator.Services
{
    using DbMigrator.Configuration;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;

    public class MongoDbService : IMongoDbService
    {
        private readonly IMongoDatabase _database;
        private readonly MongoDbSettings _settings;

        public MongoDbService(IOptions<MongoDbSettings> settings)
        {
            _settings = settings.Value;
            var client = new MongoClient(_settings.ConnectionString);
            _database = client.GetDatabase(_settings.DatabaseName);
        }

        public IMongoDatabase GetDatabase() => _database;

        public async Task<bool> TestConnectionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _database.RunCommandAsync((Command<MongoDB.Bson.BsonDocument>)"{ping:1}", cancellationToken: cancellationToken);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

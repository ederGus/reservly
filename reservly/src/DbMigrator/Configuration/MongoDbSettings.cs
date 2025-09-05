namespace DbMigrator.Configuration
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = "mongodb://root:rootpassword@localhost:27017";

        public string DatabaseName { get; set; } = "reservly";

        public int TimeoutMinutes { get; set; } = 30;
    }
}

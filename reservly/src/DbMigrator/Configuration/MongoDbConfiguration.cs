namespace DbMigrator.Configuration
{
    using DbMigrator.Models.Enums;
    using DbMigrator.Models;
    using MongoDB.Bson.Serialization.Serializers;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson;

    public static class MongoDbConfiguration
    {
        private static bool _isConfigured = false;

        public static void Configure()
        {
            if (_isConfigured)
            {
                return;
            }

            BsonSerializer.RegisterSerializer(typeof(ReservationStatus), new EnumSerializer<ReservationStatus>(BsonType.String));

            BsonClassMap.RegisterClassMap<ReservationDocument>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id);
            });

            BsonClassMap.RegisterClassMap<MigrationRecord>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id);
            });

            _isConfigured = true;
        }
    }
}

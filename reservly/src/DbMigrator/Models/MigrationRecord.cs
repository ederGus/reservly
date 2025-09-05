namespace DbMigrator.Models
{
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Bson;

    public class MigrationRecord
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = default!;

        public DateTime AppliedAtUtc { get; set; }
    }
}

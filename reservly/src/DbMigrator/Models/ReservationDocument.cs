namespace DbMigrator.Models
{
    using DbMigrator.Models.Enums;
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Bson;

    public class ReservationDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string EventId { get; set; } = default!;

        public string SessionId { get; set; } = default!;

        public string UserId { get; set; } = default!;

        [BsonRepresentation(BsonType.String)]
        public ReservationStatus Status { get; set; } = default!;

        public DateTime? HoldExpiresAt { get; set; }

        public string? QrToken { get; set; }
    }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cqrs.Core.Events;

public record EventModel(
    Guid AggregateIdentifier,
    string AggregateType,
    int Version,
    string EventType,
    BaseEvent EventData,
    DateTime TimeStamp)
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cqrs.Core.Events;

public record EventModel(
    [property: BsonId]
    [property: BsonRepresentation(BsonType.ObjectId)]
    string Id,
    DateTime TimeStamp,
    Guid AggregateIdentifier,
    string AggregateType,
    int Version,
    string EventType,
    BaseEvent EventData);
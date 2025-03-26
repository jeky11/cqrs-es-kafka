using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cqrs.Core.Messages;

public abstract record Message(
    [property: BsonGuidRepresentation(GuidRepresentation.Standard)]
    Guid Id)
{ }
using Cqrs.Core.Events;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Post.Common.Events;

public record CommentUpdatedEvent(
    Guid Id,
    [property: BsonGuidRepresentation(GuidRepresentation.Standard)]
    Guid CommentId,
    string Comment,
    string UserName,
    DateTime EditDate)
    : BaseEvent(Id, nameof(CommentUpdatedEvent))
{ }
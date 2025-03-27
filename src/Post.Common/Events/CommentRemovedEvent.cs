using Cqrs.Core.Events;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Post.Common.Events;

public record CommentRemovedEvent(
    Guid Id,
    [property: BsonGuidRepresentation(GuidRepresentation.Standard)]
    Guid CommentId) : BaseEvent(Id, nameof(CommentRemovedEvent))
{ }
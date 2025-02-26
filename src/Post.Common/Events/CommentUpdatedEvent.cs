using Cqrs.Core.Events;

namespace Post.Common.Events;

public record CommentUpdatedEvent(Guid Id, Guid CommentId, string Comment, string UserName, DateTime EditDate)
    : BaseEvent(Id, nameof(CommentUpdatedEvent))
{ }
using Cqrs.Core.Events;

namespace Post.Common.Events;

public record CommentUpdatedEvent(Guid CommentId, string Comment, string UserName, DateTime EditDate) : BaseEvent(nameof(CommentUpdatedEvent))
{ }
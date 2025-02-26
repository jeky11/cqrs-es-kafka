using Cqrs.Core.Events;

namespace Post.Common.Events;

public record CommentRemovedEvent(Guid Id, Guid CommentId) : BaseEvent(Id, nameof(CommentRemovedEvent))
{ }
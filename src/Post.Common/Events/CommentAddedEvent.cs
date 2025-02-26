using Cqrs.Core.Events;

namespace Post.Common.Events;

public record CommentAddedEvent(Guid Id, Guid CommentId, string Comment, string UserName, DateTime CommentDate)
    : BaseEvent(Id, nameof(CommentAddedEvent))
{ }
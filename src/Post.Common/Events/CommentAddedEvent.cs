using Cqrs.Core.Events;

namespace Post.Common.Events;

public record CommentAddedEvent(Guid CommentId, string Comment, string UserName, DateTime CommentDate) : BaseEvent(nameof(CommentAddedEvent))
{ }
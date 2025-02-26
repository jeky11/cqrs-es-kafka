using Cqrs.Core.Events;

namespace Post.Common.Events;

public record MessageUpdatedEvent(Guid Id, string Message) : BaseEvent(Id, nameof(MessageUpdatedEvent))
{ }
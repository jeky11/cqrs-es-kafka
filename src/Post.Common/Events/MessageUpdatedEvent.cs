using Cqrs.Core.Events;

namespace Post.Common.Events;

public record MessageUpdatedEvent(string Message) : BaseEvent(nameof(MessageUpdatedEvent))
{ }
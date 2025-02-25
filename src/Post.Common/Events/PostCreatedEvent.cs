using Cqrs.Core.Events;

namespace Post.Common.Events;

public record PostCreatedEvent(string Author, string Message, DateTime DatePosted) : BaseEvent(nameof(PostCreatedEvent))
{ }
using Cqrs.Core.Events;

namespace Post.Common.Events;

public record PostRemovedEvent(Guid Id) : BaseEvent(Id, nameof(PostRemovedEvent))
{ }
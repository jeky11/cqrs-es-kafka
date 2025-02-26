using Cqrs.Core.Events;

namespace Post.Common.Events;

public record PostLikedEvent(Guid Id) : BaseEvent(Id, nameof(PostLikedEvent))
{ }
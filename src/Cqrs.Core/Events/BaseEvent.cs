using Cqrs.Core.Messages;

namespace Cqrs.Core.Events;

public abstract record BaseEvent(Guid Id, string Type) : Message(Id)
{
    public int Version { get; set; }
}
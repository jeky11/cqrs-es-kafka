using Cqrs.Core.Messages;

namespace Cqrs.Core.Events;

public abstract record BaseEvent(string Type) : Message
{
    public int Version { get; set; }
}
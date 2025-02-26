using Cqrs.Core.Messages;

namespace Cqrs.Core.Commands;

public abstract record BaseCommand(Guid Id) : Message(Id)
{ }
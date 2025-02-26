using Cqrs.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record EditMessageCommand(Guid Id, string Message) : BaseCommand(Id)
{ }
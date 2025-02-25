using Cqrs.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record EditMessageCommand(string Message) : BaseCommand
{ }
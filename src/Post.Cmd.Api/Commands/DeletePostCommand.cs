using Cqrs.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record DeletePostCommand(string UserName) : BaseCommand
{ }
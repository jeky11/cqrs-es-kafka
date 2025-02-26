using Cqrs.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record DeletePostCommand(Guid Id, string UserName) : BaseCommand(Id)
{ }
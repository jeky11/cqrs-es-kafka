using Cqrs.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record LikePostCommand(Guid Id) : BaseCommand(Id)
{ }
using Cqrs.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record AddCommentCommand(Guid Id, string Comment, string UserName) : BaseCommand(Id)
{ }
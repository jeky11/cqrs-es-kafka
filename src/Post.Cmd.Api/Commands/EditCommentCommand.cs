using Cqrs.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record EditCommentCommand(Guid CommentId, string Comment, string UserName) : BaseCommand
{ }
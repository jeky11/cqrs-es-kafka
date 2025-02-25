using Cqrs.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record RemoveCommentCommand(Guid CommentId, string UserName) : BaseCommand
{
}
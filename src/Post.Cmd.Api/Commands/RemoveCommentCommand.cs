using Cqrs.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record RemoveCommentCommand(Guid Id, Guid CommentId, string UserName) : BaseCommand(Id)
{ }
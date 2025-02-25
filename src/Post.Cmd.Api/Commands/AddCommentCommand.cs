using Cqrs.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record AddCommentCommand(string Comment, string UserName) : BaseCommand
{ }
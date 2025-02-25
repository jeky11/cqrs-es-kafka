using Cqrs.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record NewPostCommand(string Author, string Message) : BaseCommand
{
}